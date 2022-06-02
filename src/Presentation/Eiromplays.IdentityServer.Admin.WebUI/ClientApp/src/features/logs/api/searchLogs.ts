import {
  axios,
  ExtractFnReturnType,
  QueryConfig,
  PaginationFilter,
  PaginationResponse,
} from 'eiromplays-ui';
import { useQuery } from 'react-query';

import { Log } from '../types';

export type SearchLogsDTO = PaginationFilter;

export const searchLogs = (data: SearchLogsDTO): Promise<PaginationResponse<Log>> => {
  return axios.post('/logs/search', data);
};

type QueryFnType = typeof searchLogs;

type UseSearchLogsOptions = {
  data: SearchLogsDTO;
  config?: QueryConfig<QueryFnType>;
};

export const useSearchLogs = ({ data, config }: UseSearchLogsOptions) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['search-logs', data.pageNumber],
    queryFn: () => searchLogs(data),
  });
};
