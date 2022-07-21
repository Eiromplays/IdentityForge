import { useMutation } from '@tanstack/react-query';
import { MutationConfig, queryClient, axios, MessageResponse } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { Role } from '@/features/roles';

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
    onMutate: async (updatingRole) => {
      await queryClient.cancelQueries(['role', updatingRole?.data?.id]);

      const previousRole = queryClient.getQueryData<Role>(['role', updatingRole?.data?.id]);

      queryClient.setQueryData(['role', updatingRole?.data?.id], {
        ...previousRole,
        ...updatingRole.data,
        id: updatingRole.data.id,
      });

      return { previousRole };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to update role');
      toast.error(error.response?.data);
      if (context?.previousRole) {
        queryClient.setQueryData(['role', context.previousRole.id], context.previousRole);
      }
    },
    onSuccess: async (response, variables) => {
      await queryClient.refetchQueries(['role', variables.data?.id]);
      toast.success(response.message);
    },
    ...config,
    mutationFn: updateRole,
  });
};
