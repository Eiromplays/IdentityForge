import { MutationConfig, queryClient, axios, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type UpdateRoleDTO = {
  data: {
    id: string;
    name: string;
    description: string;
  };
};

export const updateRole = async ({ data }: UpdateRoleDTO): Promise<MessageResponse> => {
  return axios.post(`/roles`, data);
};

type UseUpdateRoleOptions = {
  config?: MutationConfig<typeof updateRole>;
};

export const useUpdateRole = ({ config }: UseUpdateRoleOptions = {}) => {
  return useMutation({
    onSuccess: async (response, variables) => {
      await queryClient.invalidateQueries(['role', variables.data.id]);
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to update role');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateRole,
  });
};
