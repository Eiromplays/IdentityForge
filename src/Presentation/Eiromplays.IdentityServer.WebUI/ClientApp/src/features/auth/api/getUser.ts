import { axios } from '@/lib/axios';
import { Claim } from '@/types';

import { AuthUser } from '../types';

export const getUser = async (): Promise<AuthUser | null> => {
  const userSessionInfo = (await axios.get('/bff/user')) as Claim[];

  if (!userSessionInfo) silentLogin();

  if (process.env.NODE_ENV.toLowerCase() || 'development' === 'development') {
    const userDiagnostics = await axios.get('/bff/diagnostics');
    console.log(userDiagnostics);
  }

  const nameDictionary =
    userSessionInfo?.find((claim: Claim) => claim.type === 'name') ??
    userSessionInfo?.find((claim: Claim) => claim.type === 'sub');

  const user: AuthUser = {
    id: userSessionInfo?.find((claim: Claim) => claim.type === 'sub')?.value ?? '',
    username: nameDictionary?.value ?? '',
    email: userSessionInfo?.find((claim: Claim) => claim.type === 'email')?.value ?? '',
    gravatarEmail:
      userSessionInfo?.find((claim: Claim) => claim.type === 'gravatar_email')?.value ?? '',
    profilePicture: userSessionInfo?.find((claim: Claim) => claim.type === 'picture')?.value ?? '',
    roles:
      (userSessionInfo
        ?.filter((claim: Claim) => claim.type === 'role')
        .map((claim: Claim) => claim.value.toLowerCase()) as unknown as string[]) ?? [],
    logoutUrl:
      userSessionInfo?.find((claim: Claim) => claim.type === 'bff:logout_url')?.value ??
      '/bff/logout',
    updated_at: new Date(
      userSessionInfo?.find((claim: Claim) => claim.type === 'updated_at')?.value ?? ''
    ),
    created_at: new Date(
      userSessionInfo?.find((claim: Claim) => claim.type === 'created_at')?.value ?? ''
    ),
  };

  if (user.id) return user;

  return null;
};

export const silentLogin = () => {
  const useSilentLogin = process.env.REACT_APP_USE_SILENT_LOGIN;

  // TODO: Find a better solution for this, both the useSilentLogin and the url checking
  if (useSilentLogin?.toLowerCase() === 'false' || window.location.href.includes('logout')) {
    console.log('SILENT LOGIN DISABLED');
    return;
  }

  const bffSilentLoginIframe = document.createElement('iframe');
  document.body.append(bffSilentLoginIframe);

  bffSilentLoginIframe.src = '/bff/silent-login';

  window.addEventListener('message', (e) => {
    if (e.data && e.data.source === 'bff-silent-login' && e.data.isLoggedIn) {
      // we now have a user logged in silently, so reload this window

      window.location.reload();
    }
  });
};
