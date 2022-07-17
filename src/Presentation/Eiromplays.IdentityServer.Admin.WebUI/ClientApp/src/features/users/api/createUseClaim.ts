import { useSearch } from '@tanstack/react-location';
import {
  axios,
  defaultPageIndex,
  defaultPageSize,
  MessageResponse,
  MutationConfig,
  PaginationResponse,
  queryClient,
} from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { UserClaim } from '@/features/users';

export type CreateUserClaimDTO = {
  userId: string;
  addUserClaimRequest: {
    type: string;
    value: string;
    valueType?: string;
    issuer?: string;
  };
};

export const createUserClaim = async (data: CreateUserClaimDTO): Promise<MessageResponse> => {
  return axios.post(`/users/${data.userId}/claims`, data);
};

export type UseCreateUserClaimOptions = {
  config?: MutationConfig<typeof createUserClaim>;
};

export const useCreateUserClaim = ({ config }: UseCreateUserClaimOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (newUserClaim) => {
      await queryClient.cancelQueries([
        `search-user-claims-${newUserClaim.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousUserClaims = queryClient.getQueryData<PaginationResponse<UserClaim>>([
        `search-user-claims-${newUserClaim.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          `search-user-claims-${newUserClaim.userId}`,
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        [...(previousUserClaims?.data || []), newUserClaim.addUserClaimRequest]
      );

      return { previousUserClaims };
    },
    onError: (error, variables, context: any) => {
      toast.error('Failed to add UserClaim');
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
    onSuccess: async (response, variables) => {
      await queryClient.invalidateQueries([
        `search-user-claims-${variables.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('UserClaim added');
      toast.success(response.message);
    },
    ...config,
    mutationFn: createUserClaim,
  });
};
