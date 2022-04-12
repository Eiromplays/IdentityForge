import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';

export const deleteUser = () => {
  return axios.delete(`/personal/profile`);
};

type UseDeleteUserOptions = {
  config?: MutationConfig<typeof deleteUser>;
};

export const useDeleteUser = ({ config }: UseDeleteUserOptions = {}) => {
  return useMutation({
    onError: () => {
      toast.error('Unable to delete user');
    },
    onSuccess: () => {
      toast.success('User Deleted');
    },
    ...config,
    mutationFn: deleteUser,
  });
};
