import { axios } from '@/lib/axios';
import { Claim } from '@/types';

import { AuthUser } from '../types';

export const getUser = async (): Promise<AuthUser> => {
  const userSessionInfo = (await axios.get('/bff/user')) as { type: string; value: string }[];
  if (!userSessionInfo && silentLogin()) return getUser();
  console.log(userSessionInfo);
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
      userSessionInfo?.find((claim: Claim) => claim.type === 'gravatarEmail')?.value ?? '',
    profilePicture: userSessionInfo?.find((claim: Claim) => claim.type === 'picture')?.value ?? '',
    roles:
      (userSessionInfo
        ?.filter((claim: Claim) => claim.type === 'role')
        .map((claim: Claim) => claim.value.toLowerCase()) as unknown as string[]) ?? [],
    logoutUrl:
      userSessionInfo?.find((claim: Claim) => claim.type === 'bff:logout_url')?.value ??
      '/bff/logout',
  };

  return user;
};

export const silentLogin = (): boolean => {
  const bffSilentLoginIframe = document.createElement('iframe');

  document.body.append(bffSilentLoginIframe);

  bffSilentLoginIframe.src = '/bff/silent-login';

  let silentLoginSuccess = false;

  window.addEventListener('message', (e) => {
    if (e.data && e.data.source === 'bff-silent-login' && e.data.isLoggedIn) {
      // we now have a user logged in silently, so reload this window

      history.go(0);
      silentLoginSuccess = true;
    } else {
      window.location.assign('/bff/login');
    }
  });

  return silentLoginSuccess;
};
