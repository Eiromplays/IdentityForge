import { axios, QueryConfig, ExtractFnReturnType } from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { User } from '../types';

export const getUsers = (): Promise<User[]> => {
  return axios.get(`/users`);
};

type QueryFnType = typeof getUsers;

type UseUsersOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useUsers = ({ config }: UseUsersOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['users'],
    queryFn: () => getUsers(),
  });
};
