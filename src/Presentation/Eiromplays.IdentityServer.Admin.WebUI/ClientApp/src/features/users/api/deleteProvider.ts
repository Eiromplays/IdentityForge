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
import { UserProvider } from '@/features/users';

export type DeleteUserClaimDTO = {
  userId: string;
  removeLoginRequest: {
    loginProvider: string;
    providerKey: string;
  };
};

export const deleteUserProvider = (data: DeleteUserClaimDTO): Promise<MessageResponse> => {
  return axios.post(`/users/${data.userId}/providers-delete`, data);
};

export type UseDeleteUserProviderOptions = {
  config?: MutationConfig<typeof deleteUserProvider>;
};

export const useDeleteUserProvider = ({ config }: UseDeleteUserProviderOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedProvider) => {
      await queryClient.cancelQueries([
        `search-user-providers-${deletedProvider.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousUserProviders = queryClient.getQueryData<PaginationResponse<UserProvider>>([
        `search-user-providers-${deletedProvider.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          `search-user-providers-${deletedProvider.userId}`,
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousUserProviders?.data?.filter(
          (userProvider) =>
            userProvider.providerKey !== deletedProvider.removeLoginRequest.providerKey
        ) ?? []
      );

      return { previousUserProviders };
    },
    onError: (_, variables, context: any) => {
      if (context?.previousUserProviders) {
        queryClient.setQueryData(
          [
            `search-user-providers-${variables.userId}`,
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousUserProviders
        );
      }
    },
    onSuccess: async (response, variables) => {
      await queryClient.invalidateQueries([
        `search-user-providers-${variables.userId}`,
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success(response.message);
    },
    ...config,
    mutationFn: deleteUserProvider,
  });
};
