import { useSearch } from '@tanstack/react-location';
import { useMutation } from '@tanstack/react-query';
import {
  axios,
  defaultPageIndex,
  defaultPageSize,
  MutationConfig,
  PaginationResponse,
  queryClient,
} from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { RoleClaim } from '@/features/roles';

export type UpdateRoleClaimDTO = {
  roleId: string;
  claimId: number;
  data: {
    type: string;
    value: string;
  };
};

export const updateRoleClaim = async ({ roleId, claimId, data }: UpdateRoleClaimDTO) => {
  return axios.put(`/roles/${roleId}/claims/${claimId}`, { UpdateRoleClaimRequest: data });
};

export type UseUpdateRoleClaimOptions = {
  config?: MutationConfig<typeof updateRoleClaim>;
};

export const useUpdateRoleClaim = ({ config }: UseUpdateRoleClaimOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (updatingRoleClaim) => {
      await queryClient.cancelQueries([
        'search-role-claims',
        updatingRoleClaim.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousRoleClaims = queryClient.getQueryData<PaginationResponse<RoleClaim>>([
        'search-role-claims',
        updatingRoleClaim.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-role-claims',
          updatingRoleClaim.roleId,
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousRoleClaims?.data?.filter(
          (roleClaim) => roleClaim.id !== updatingRoleClaim.claimId
        ) ?? []
      );

      return { previousRoleClaims };
    },
    onError: (error, variables, context: any) => {
      toast.error('Failed to update role claim');
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
    onSuccess: async (_, variables) => {
      await queryClient.refetchQueries([
        'search-role-claims',
        variables.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success(`Role claim has been updated successfully`);
    },
    ...config,
    mutationFn: updateRoleClaim,
  });
};
