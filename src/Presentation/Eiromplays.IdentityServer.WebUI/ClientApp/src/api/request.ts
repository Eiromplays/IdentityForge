import axios, { Method, AxiosResponse } from 'axios';

const api = axios.create();

const request = <T>(method: Method, url: string, params: any): Promise<AxiosResponse<T>> => {
  return api.request<T>({
    method,
    url,
    params,
    headers: {
      'X-CSRF': '1',
    },
  });
};

export const defaultQueryFn = async ({ queryKey }: any): Promise<unknown> => {
  const response = await request(queryKey[0], queryKey[1], queryKey[2]);

  return response;
};
