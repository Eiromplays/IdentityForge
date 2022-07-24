import { useSearch } from '@tanstack/react-location';
import { useMutation } from '@tanstack/react-query';
import {
  axios,
  defaultPageIndex,
  defaultPageSize,
  MessageResponse,
  MutationConfig,
  PaginationResponse,
  queryClient,
} from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { UserClaim } from '@/features/users';

export type CreateRoleClaimDTO = {
  roleId: string;
  addRoleClaimRequest: {
    type: string;
    value: string;
  };
};

export const createRoleClaim = async (data: CreateRoleClaimDTO): Promise<MessageResponse> => {
  return axios.post(`/roles/${data.roleId}/claims`, data);
};

export type UseCreateRoleClaimOptions = {
  config?: MutationConfig<typeof createRoleClaim>;
};

export const useCreateRoleClaim = ({ config }: UseCreateRoleClaimOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newUserClaim) => {
      await queryClient.cancelQueries([
        'search-role-claims',
        newUserClaim.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousRoleClaims = queryClient.getQueryData<PaginationResponse<UserClaim>>([
        'search-role-claims',
        newUserClaim.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-role-claims',
          newUserClaim.roleId,
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        [...(previousRoleClaims?.data || []), newUserClaim.addRoleClaimRequest]
      );

      return { previousRoleClaims };
    },
    onError: (error, variables, context: any) => {
      toast.error('Failed to add role claim');
      toast.error(error.response?.data);
      if (context?.previousRoleClaims) {
        queryClient.setQueryData(
          [
            'search-role-claims',
            variables.roleId,
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousRoleClaims
        );
      }
    },
    onSuccess: async (response, variables) => {
      await queryClient.invalidateQueries([
        'search-role-claims',
        variables.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('Role claim added');
      toast.success(response.message);
    },
    ...config,
    mutationFn: createRoleClaim,
  });
};
