import { axios } from '@/lib/axios';
import { Claim } from '@/types';

import { AuthUser } from '../types';

export const getUser = async (): Promise<AuthUser> => {
  const userSessionInfo = (await axios.get('/bff/user')) as { type: string; value: string }[];
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
