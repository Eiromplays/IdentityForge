import { useSearch } from '@tanstack/react-location';
import { useQuery } from '@tanstack/react-query';
import { ExtractFnReturnType, QueryConfig, axios } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';
import { identityServerUrl } from '@/utils/envVariables';

import { LogoutResponse } from '../types';

export type LogoutDTO = {
  logoutId: string;
};

export const getLogoutInfo = ({ logoutId }: LogoutDTO): Promise<LogoutResponse> => {
  return axios.get(`${identityServerUrl}/api/v1/account/logout?logoutId=${logoutId}`);
};

export const logoutUser = ({ logoutId }: LogoutDTO): Promise<LogoutResponse> => {
  const logoutDto: LogoutDTO = {
    logoutId: logoutId,
  };

  return axios.post(`${identityServerUrl}/api/v1/account/logout`, logoutDto);
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
