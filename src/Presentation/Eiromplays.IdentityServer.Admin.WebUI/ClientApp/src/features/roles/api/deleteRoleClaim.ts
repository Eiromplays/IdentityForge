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

export type DeleteRoleClaimDTO = {
  roleId: string;
  claimId: number;
};

export const deleteRoleClaim = ({ roleId, claimId }: DeleteRoleClaimDTO) => {
  return axios.delete(`/roles/${roleId}/claims/${claimId}`);
};

export type UseDeleteRoleClaimOptions = {
  config?: MutationConfig<typeof deleteRoleClaim>;
};

export const useDeleteRoleClaim = ({ config }: UseDeleteRoleClaimOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedRoleClaim) => {
      await queryClient.cancelQueries([
        'search-role-claims',
        deletedRoleClaim.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousRoleClaims = queryClient.getQueryData<PaginationResponse<RoleClaim>>([
        'search-role-claims',
        deletedRoleClaim.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-role-claims',
          deletedRoleClaim.roleId,
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousRoleClaims?.data?.filter(
          (roleClaim) => roleClaim.id !== deletedRoleClaim.claimId
        ) ?? []
      );

      return { previousRoleClaims };
    },
    onError: (_, variables, context: any) => {
      if (context?.previousUserClaims) {
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
      await queryClient.invalidateQueries([
        'search-role-claims',
        variables.roleId,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('Role claim deleted');
    },
    ...config,
    mutationFn: deleteRoleClaim,
  });
};
