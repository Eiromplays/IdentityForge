import { axios } from '@/lib/axios';
import { Claim } from '@/types';

import { AuthUser, UserSessionInfo, UserData } from '../types';

export const getAuthUser = async (): Promise<AuthUser> => {
  const userSessionInfo = await getUserSessionInfo();

  const authUser: AuthUser = {
    data: await getUserData(userSessionInfo.id),
    sessionInfo: userSessionInfo,
  };

  return authUser;
};

export const getUserSessionInfo = async (): Promise<UserSessionInfo> => {
  const userSessionInfoClaims = (await axios.get('/bff/user')) as { type: string; value: string }[];

  const nameDictionary =
    userSessionInfoClaims?.find((claim: Claim) => claim.type === 'name') ??
    userSessionInfoClaims?.find((claim: Claim) => claim.type === 'sub');

  const userSessionInfo: UserSessionInfo = {
    id: userSessionInfoClaims?.find((claim: Claim) => claim.type === 'sub')?.value,
    username: nameDictionary?.value,
    logoutUrl:
      userSessionInfoClaims?.find((claim: Claim) => claim.type === 'bff:logout_url')?.value ??
      '/bff/logout',
  };

  return userSessionInfo;
};

export const getUserData = (id: string | undefined): Promise<UserData> => {
  return axios.get(`/users/${id}`);
};
