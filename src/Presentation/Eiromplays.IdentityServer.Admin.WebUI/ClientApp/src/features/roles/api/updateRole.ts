import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig, queryClient } from '@/lib/react-query';

export type UpdateRoleDTO = {
  roleId: string;
  data: {
    name: string;
    description: string;
  };
};

export const updateRole = async ({ roleId, data }: UpdateRoleDTO) => {
  return axios.put(`/roles/${roleId}`, { Data: data });
};

type UseUpdateRoleOptions = {
  config?: MutationConfig<typeof updateRole>;
};

export const useUpdateRole = ({ config }: UseUpdateRoleOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      queryClient.invalidateQueries('roles');
      toast.success('Role Updated');
    },
    onError: (error) => {
      toast.error('Failed to update role');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: updateRole,
  });
};
