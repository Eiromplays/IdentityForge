import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

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

type UseUpdateUserOptions = {
  config?: MutationConfig<typeof updateUser>;
};

export const useUpdateUser = ({ config }: UseUpdateUserOptions = {}) => {
  const updateProfileMutation = useMutation({
    onSuccess: async (_, variables) => {
      await queryClient.refetchQueries(['user', variables.userId]);
      toast.success(`${variables.data?.username} has been updated successfully`);
    },
    onError: (error) => {
      toast.error('Failed to update user');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateUser,
  });

  return { updateUserMutation: updateProfileMutation };
};
