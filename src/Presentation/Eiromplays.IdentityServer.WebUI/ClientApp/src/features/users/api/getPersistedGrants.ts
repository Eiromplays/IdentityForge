import { useQuery } from 'react-query';

import { axios } from '@/lib/axios';
import { ExtractFnReturnType, QueryConfig } from '@/lib/react-query';

import { PersistedGrant } from '../types';

export type GetPersistedGrants = {
  subjectId?: string;
};

export const getPersistedGrants = ({
  subjectId,
}: GetPersistedGrants): Promise<PersistedGrant[]> => {
  return axios.get(`/persisted-grants/subjects/${subjectId}`);
};

type QueryFnType = typeof getPersistedGrants;

type UsePersistedGrantsOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const usePersistedGrants = (
  { subjectId }: GetPersistedGrants,
  { config }: UsePersistedGrantsOptions = {}
) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['persisted-grants'],
    queryFn: () => getPersistedGrants({ subjectId }),
  });
};
