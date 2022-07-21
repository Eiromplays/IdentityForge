import { useQuery } from '@tanstack/react-query';
import { axios, QueryConfig, ExtractFnReturnType } from 'eiromplays-ui';

import { User } from '../types';

export type GetUserDTO = {
  userId: string;
};

export const getUser = ({ userId }: GetUserDTO): Promise<User> => {
  return axios.get(`/users/${userId}`);
};

type QueryFnType = typeof getUser;

type UseUserOptions = {
  userId: string;
  config?: QueryConfig<QueryFnType>;
};

export const useUser = ({ userId, config }: UseUserOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['user', userId],
    queryFn: () => getUser({ userId }),
  });
};
