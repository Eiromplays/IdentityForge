import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig } from 'eiromplays-ui';
import { toast } from 'react-toastify';

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
