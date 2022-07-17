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

export type UpdateUserClaimDTO = {
  userId: string;
  data: {
    newType: string;
    newValue: string;
    oldType: string;
    oldValue: string;
    valueType?: string;
    issuer?: string;
  };
};

export const updateUserClaim = async ({ userId, data }: UpdateUserClaimDTO) => {
  return axios.put(`/users/${userId}/claims`, { UpdateUserClaimRequest: data });
};

export type UseUpdateUserClaimOptions = {
  config?: MutationConfig<typeof updateUserClaim>;
};

export const useUpdateUserClaim = ({ config }: UseUpdateUserClaimOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (updatingUserClaim) => {
      await queryClient.cancelQueries([
        `search-user-claims-${updatingUserClaim.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousUserClaims = queryClient.getQueryData<PaginationResponse<UserClaim>>([
        `search-user-claims-${updatingUserClaim.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          `search-user-claims-${updatingUserClaim.userId}`,
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousUserClaims?.data?.filter(
          (userClaim) =>
            userClaim.type !== updatingUserClaim.data?.oldType &&
            userClaim.value !== updatingUserClaim.data?.oldValue
        ) ?? []
      );

      return { previousUserClaims };
    },
    onError: (error, variables, context: any) => {
      toast.error('Failed to update UserClaim');
      toast.error(error.response?.data);
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
      await queryClient.refetchQueries([
        `search-user-claims-${variables.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success(`UserClaim has been updated successfully`);
    },
    ...config,
    mutationFn: updateUserClaim,
  });
};
