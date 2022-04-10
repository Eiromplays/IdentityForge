import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

import { LogoutResponse } from '../types';

export type LogoutDTO = {
  logoutId: string;
};

export const getLogoutInfo = (): Promise<LogoutResponse> => {
  const logoutId = getLogoutId();

  return axios.get(`https://localhost:7001/spa/Logout?logoutId=${logoutId}`, {
    withCredentials: true,
  });
};

export const logoutUser = (): Promise<LogoutResponse> => {
  const logoutId = getLogoutId();

  const logoutDto: LogoutDTO = {
    logoutId: logoutId,
  };

  return axios.post(`https://localhost:7001/spa/Logout`, logoutDto, {
    withCredentials: true,
  });
};

const getLogoutId = (): string => {
  // TODO: I need to refactor this, one day :)

  let logoutId = '';
  const idx = location.href.toLowerCase().indexOf('?logoutid=');
  if (idx > 0) {
    logoutId = location.href.substring(idx + 10);
  }

  return logoutId;
};

type QueryFnType = typeof getLogoutInfo;

type UseLogoutOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useLogout = ({ config }: UseLogoutOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['logoutInfo'],
    queryFn: () => getLogoutInfo(),
  });
};
