import { useSearch } from '@tanstack/react-location';
import {
  axios,
  defaultPageIndex,
  defaultPageSize,
  MutationConfig,
  PaginationResponse,
  queryClient,
} from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { UserClaim } from '@/features/users';

export type DeleteUserClaimDTO = {
  userId: string;
  removeUserClaimRequest: {
    type: string;
    value: string;
  };
};

export const deleteUserClaim = (data: DeleteUserClaimDTO) => {
  return axios.post(`/users/${data.userId}/claims-delete`, data);
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
          (userClaim) =>
            userClaim.value !== deletedUserClaim.removeUserClaimRequest.value &&
            userClaim.type !== deletedUserClaim.removeUserClaimRequest.type
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
      toast.success('UserClaim deleted');
    },
    ...config,
    mutationFn: deleteUserClaim,
  });
};
