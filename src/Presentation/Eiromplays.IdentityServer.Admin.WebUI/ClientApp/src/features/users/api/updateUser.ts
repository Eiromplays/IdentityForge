import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { User } from '@/features/users';

export type UpdateUserDTO = {
  userId: string;
  data: {
    username: string;
    displayName: string;
    firstName: string;
    lastName: string;
    phoneNumber: string;
    email: string;
    gravatarEmail: string;
    image: any;
    deleteCurrentImage?: boolean;
    revokeUserSessions?: boolean;
    emailConfirmed?: boolean;
    phoneNumberConfirmed?: boolean;
    twoFactorEnabled?: boolean;
    lockoutEnabled?: boolean;
    isActive?: boolean;
  };
};

// even if you decide not to use precaching. See https://cra.link/PWA
const toBase64 = (file: File) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

export const updateUser = async ({ userId, data }: UpdateUserDTO) => {
  if (data.image instanceof File) {
    const fileExtension = `.${data.image.name.slice(
      ((data.image.name.lastIndexOf('.') - 1) >>> 0) + 2
    )}`;
    data.image = {
      data: await toBase64(data.image),
      extension: fileExtension,
      name: data.image.name.replace(fileExtension, ''),
    };
    data.deleteCurrentImage = data.image ? true : data.deleteCurrentImage;
  }

  return axios.put(`/users/${userId}`, { UpdateUserRequest: data });
};

export type UseUpdateUserOptions = {
  config?: MutationConfig<typeof updateUser>;
};

export const useUpdateUser = ({ config }: UseUpdateUserOptions = {}) => {
  return useMutation({
    onMutate: async (updatingUser) => {
      await queryClient.cancelQueries(['user', updatingUser?.userId]);

      const previousUser = queryClient.getQueryData<User>(['user', updatingUser?.userId]);

      queryClient.setQueryData(['user', updatingUser?.userId], {
        ...previousUser,
        ...updatingUser.data,
        id: updatingUser.userId,
      });

      return { previousUser };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to update user');
      toast.error(error.response?.data);
      if (context?.previousUser) {
        queryClient.setQueryData(['user', context.previousUser.id], context.previousUser);
      }
    },
    onSuccess: async (_, variables) => {
      await queryClient.refetchQueries(['user', variables.userId]);
      toast.success(`${variables.data?.username} has been updated successfully`);
    },
    ...config,
    mutationFn: updateUser,
  });
};
