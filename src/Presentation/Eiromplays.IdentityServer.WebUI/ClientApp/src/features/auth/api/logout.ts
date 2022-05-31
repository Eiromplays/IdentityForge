import { useSearch } from '@tanstack/react-location';
import { ExtractFnReturnType, QueryConfig, axios } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { LocationGenerics } from '@/App';
import { identityServerUrl } from '@/utils/envVariables';

import { LogoutResponse } from '../types';

export type LogoutDTO = {
  logoutId: string;
};

export const getLogoutInfo = ({ logoutId }: LogoutDTO): Promise<LogoutResponse> => {
  return axios.get(`${identityServerUrl}/spa/Logout?logoutId=${logoutId}`);
};

export const logoutUser = ({ logoutId }: LogoutDTO): Promise<LogoutResponse> => {
  const logoutDto: LogoutDTO = {
    logoutId: logoutId,
  };

  return axios.post(`${identityServerUrl}/spa/Logout`, logoutDto);
};

type QueryFnType = typeof getLogoutInfo;

type UseLogoutOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useLogout = ({ config }: UseLogoutOptions = {}) => {
  const { logoutId } = useSearch<LocationGenerics>();

  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['logout-info'],
    queryFn: () => getLogoutInfo({ logoutId: logoutId || '' }),
  });
};
