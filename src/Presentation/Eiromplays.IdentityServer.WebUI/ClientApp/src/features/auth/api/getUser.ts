import { axios } from '@/lib/axios';

import { AuthUser, UserSessionInfo, UserData } from '../types';

export const getAuthUser = async (): Promise<AuthUser> => {
  const authUser: AuthUser = {
    data: await getUserData(),
    sessionInfo: await getUserSessionInfo(),
  };

  return authUser;
};

export const getUserSessionInfo = async (): Promise<UserSessionInfo> => {
  const userSessionInfoClaims = (await axios.get('/bff/user')) as { type: string; value: string }[];

  const nameDictionary =
    userSessionInfoClaims?.find((claim: any) => claim.type === 'name') ??
    userSessionInfoClaims?.find((claim: any) => claim.type === 'sub');

  const userSessionInfo: UserSessionInfo = {
    username: nameDictionary?.value,
    logoutUrl:
      userSessionInfoClaims?.find((claim: any) => claim.type === 'bff:logout_url')?.value ??
      '/bff/logout',
  };

  return userSessionInfo;
};

export const getUserData = (): Promise<UserData> => {
  return axios.get('/users');
};
