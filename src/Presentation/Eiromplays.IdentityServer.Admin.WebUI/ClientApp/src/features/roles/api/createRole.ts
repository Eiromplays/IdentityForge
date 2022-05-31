import { MutationConfig, axios, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type CreateRoleDTO = {
  data: {
    name: string;
    description: string;
  };
};

export const createRole = async ({ data }: CreateRoleDTO): Promise<MessageResponse> => {
  return axios.post(`/roles`, data);
};

type UseUpdateRoleOptions = {
  config?: MutationConfig<typeof createRole>;
};

export const useCreateRole = ({ config }: UseUpdateRoleOptions = {}) => {
  return useMutation({
    onSuccess: async (response) => {
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to create role');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: createRole,
  });
};
