import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { useAuth } from '@/lib/auth';
import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';

export type UpdateProfileDTO = {
  id?: string;
  data: {
    username: string;
    email: string;
  };
};

export const updateProfile = ({ data, id }: UpdateProfileDTO) => {
  return axios.put(`/users/${id}`, data);
};

type UseUpdateProfileOptions = {
  config?: MutationConfig<typeof updateProfile>;
};

export const useUpdateProfile = ({ config }: UseUpdateProfileOptions = {}) => {
  const { refetchUser } = useAuth();
  return useMutation({
    onSuccess: () => {
      toast.success('User Updated');
      refetchUser();
    },
    onError: (error) => {
      toast.error('Failed to update user');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateProfile,
  });
};
