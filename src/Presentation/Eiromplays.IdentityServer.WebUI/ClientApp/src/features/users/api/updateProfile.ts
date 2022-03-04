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
    profilePicture: File;
  };
};

export const updateProfile = async ({ data, id }: UpdateProfileDTO) => {
  const formData = new FormData();
  formData.append('ProfilePicture', data.profilePicture);

  const updateUserResponse = await axios.put(`/users/${id}`, data);

  if (!data.profilePicture) return updateUserResponse;

  const updateProfilePictureResponse = await axios.post(`/users/${id}/picture`, formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });

  return await Promise.all([updateProfilePictureResponse, updateUserResponse]);
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
