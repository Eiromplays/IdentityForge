import { axios, MessageResponse, MutationConfig, useAuth } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export type UpdateProfileDTO = {
  data: {
    id: string;
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    gravatarEmail: string;
    image: any;
    deleteCurrentImage?: boolean;
    phoneNumber: string;
  };
};

const toBase64 = (file: File) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

export type UpdateProfileResponse = MessageResponse & {
  logoutRequired: boolean;
  returnUrl: string;
};

export const updateProfile = async ({ data }: UpdateProfileDTO): Promise<UpdateProfileResponse> => {
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

  return axios.put(`${identityServerUrl}/api/v1/manage/update-profile`, data);
};

type UseUpdateProfileOptions = {
  config?: MutationConfig<typeof updateProfile>;
};

export const useUpdateProfile = ({ config }: UseUpdateProfileOptions = {}) => {
  const { logout } = useAuth();

  const updateProfileMutation = useMutation({
    onSuccess: async (response) => {
      console.log(response);
      toast.success(response.message);
      if (response.logoutRequired) {
        await logout();
        window.location.href = response.returnUrl;
      } else {
        window.location.href = `/bff/login?returnUrl=${window.location.pathname}`;
      }
    },
    onError: (error) => {
      toast.error('Failed to update user');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateProfile,
  });

  return { updateProfileMutation };
};
