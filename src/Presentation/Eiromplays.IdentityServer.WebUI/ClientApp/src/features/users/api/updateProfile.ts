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
    gravatarEmail: string;
    profilePicture: File;
  };
};

export type DeleteProfilePictureDTO = {
  id?: string;
};

export const updateProfile = async ({ data, id }: UpdateProfileDTO) => {
  const formData = new FormData();
  formData.append('ProfilePicture', data.profilePicture);

  const updateUserResponse = await axios.put(`/users/${id}`, data);

  if (!data.profilePicture) return updateUserResponse;

  const updateProfilePictureResponse = await axios.post(`/users/${id}/profile-picture`, formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });

  return await Promise.all([updateProfilePictureResponse, updateUserResponse]);
};

export const deleteProfilePicture = async ({ id }: DeleteProfilePictureDTO) => {
  return await axios.delete(`/users/${id}/profile-picture`);
};

type UseUpdateProfileOptions = {
  config?: MutationConfig<typeof updateProfile | typeof deleteProfilePicture>;
};

export const useUpdateProfile = ({ config }: UseUpdateProfileOptions = {}) => {
  const { logout } = useAuth();

  const updateProfileMutation = useMutation({
    onSuccess: () => {
      toast.success('User Updated');
      logout();
    },
    onError: (error) => {
      toast.error('Failed to update user');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateProfile,
  });

  const deleteProfilePictureMutation = useMutation({
    onSuccess: () => {
      toast.success('Profile picture Updated');
      logout();
    },
    onError: (error) => {
      toast.error('Failed to delete profile picture');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: deleteProfilePicture,
  });

  return { updateProfileMutation, deleteProfilePictureMutation };
};
