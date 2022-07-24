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
import { UserClaim } from '@/features/users';

export type DeleteUserClaimDTO = {
  userId: string;
  claimId: number;
};

export const deleteUserClaim = (data: DeleteUserClaimDTO) => {
  return axios.delete(`/users/${data.userId}/claims/${data.claimId}`);
};

export type UseDeleteUserOptions = {
  config?: MutationConfig<typeof deleteUserClaim>;
};

export const useDeleteUserClaim = ({ config }: UseDeleteUserOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedUserClaim) => {
      await queryClient.cancelQueries([
        `search-user-claims-${deletedUserClaim.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousUserClaims = queryClient.getQueryData<PaginationResponse<UserClaim>>([
        `search-user-claims-${deletedUserClaim.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          `search-user-claims-${deletedUserClaim.userId}`,
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousUserClaims?.data?.filter(
          (userClaim) => userClaim.id !== deletedUserClaim.claimId
        ) ?? []
      );

      return { previousUserClaims };
    },
    onError: (_, variables, context: any) => {
      if (context?.previousUserClaims) {
        queryClient.setQueryData(
          [
            `search-user-claims-${variables.userId}`,
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousUserClaims
        );
      }
    },
    onSuccess: async (_, variables) => {
      await queryClient.invalidateQueries([
        `search-user-claims-${variables.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('User claim deleted');
    },
    ...config,
    mutationFn: deleteUserClaim,
  });
};
