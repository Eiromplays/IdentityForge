import { axios } from '@/lib/axios';
import { Claim } from '@/types';
import { formatDate } from '@/utils/format';

import { AuthUser } from '../types';

export const getUser = async (): Promise<AuthUser | null> => {
  const isAuthenticated = (await axios.get(
    'https://localhost:7001/account/isAuthenticated'
  )) as boolean;

  if (!isAuthenticated) return null;

  const userSessionInfo = (await axios.get('/bff/user')) as Claim[];
  console.log(userSessionInfo);

  if (!userSessionInfo && isAuthenticated) {
    silentLogin();
  }

  const nameDictionary =
    userSessionInfo?.find((claim: Claim) => claim.type === 'name') ??
    userSessionInfo?.find((claim: Claim) => claim.type === 'sub');

  const user: AuthUser = {
    id: userSessionInfo?.find((claim: Claim) => claim.type === 'sub')?.value ?? '',
    sessionId: userSessionInfo?.find((claim: Claim) => claim.type === 'sid')?.value ?? '',
    username: nameDictionary?.value ?? '',
    firstName: userSessionInfo?.find((claim: Claim) => claim.type === 'given_name')?.value ?? '',
    lastName: userSessionInfo?.find((claim: Claim) => claim.type === 'family_name')?.value ?? '',
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
    updated_at: formatDate(
      (userSessionInfo?.find((claim: Claim) => claim.type === 'updated_at')?.value ?? 0) as number
    ),
    created_at: formatDate(
      (userSessionInfo?.find((claim: Claim) => claim.type === 'created_at')?.value ?? 0) as number
    ),
  };

  if (user.id) return user;

  return null;
};

export const silentLogin = () => {
  const useSilentLogin = process.env.REACT_APP_USE_SILENT_LOGIN;

  // TODO: Find a better solution for useSilentLogin
  if (useSilentLogin?.toLowerCase() === 'false') return;

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
