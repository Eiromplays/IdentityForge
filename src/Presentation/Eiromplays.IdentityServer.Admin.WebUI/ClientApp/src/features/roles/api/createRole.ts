import { useSearch } from '@tanstack/react-location';
import {
  MutationConfig,
  axios,
  MessageResponse,
  queryClient,
  PaginationResponse,
} from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { Role } from '@/features/roles';

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
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newRole) => {
      await queryClient.cancelQueries(['search-roles']);

      const previousRoles = queryClient.getQueryData<PaginationResponse<Role>>([
        'search-roles',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-roles', pagination?.index ?? 1, pagination?.size ?? 10],
        [...(previousRoles?.data || []), newRole.data]
      );

      return { previousRoles };
    },
    onError: (error, __, context: any) => {
      toast.error('Failed to create role');
      toast.error(error.response?.data);
      if (context?.previousRoles) {
        queryClient.setQueryData(
          ['search-roles', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousRoles
        );
      }
    },
    onSuccess: async (response) => {
      await queryClient.invalidateQueries([
        'search-roles',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success(response.message);
    },
    ...config,
    mutationFn: createRole,
  });
};
