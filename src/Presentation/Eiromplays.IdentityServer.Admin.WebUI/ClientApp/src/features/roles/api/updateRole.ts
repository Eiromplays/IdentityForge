import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig, queryClient } from '@/lib/react-query';

export type UpdateRoleDTO = {
  data: {
    id: string;
    name: string;
    description: string;
  };
};

export const updateRole = async ({ data }: UpdateRoleDTO) => {
  return axios.post(`/roles`, { Data: data });
};

type UseUpdateRoleOptions = {
  config?: MutationConfig<typeof updateRole>;
};

export const useUpdateRole = ({ config }: UseUpdateRoleOptions = {}) => {
  return useMutation({
    onSuccess: async (response) => {
      queryClient.invalidateQueries('roles');
      queryClient.invalidateQueries(`role-${response.data.RoleId}`);
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
