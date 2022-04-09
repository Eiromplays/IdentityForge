import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';

export type LogoutDTO = {
  logoutId?: string;
};

export const getLogoutInfo = ({ logoutId }: LogoutDTO): Promise<any> => {
  return axios.get(`https://localhost:7001/spa/Logout?logoutId=${logoutId}`, {
    withCredentials: true,
  });
};

export const logoutUser = ({ logoutId }: LogoutDTO): Promise<any> => {
  return axios.post(`https://localhost:7001/spa/Logout?logoutId=${logoutId}`, {
    withCredentials: true,
  });
};

type UseLogoutOptions = {
  config?: MutationConfig<typeof getLogoutInfo>;
};

export const useLogout = ({ config }: UseLogoutOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      toast.success('Successfully logged out');
    },
    onError: (error) => {
      toast.error('Failed to update user');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: getLogoutInfo,
  });
};
