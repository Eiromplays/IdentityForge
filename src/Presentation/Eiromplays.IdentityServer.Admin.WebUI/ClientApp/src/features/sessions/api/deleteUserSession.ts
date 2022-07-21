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

import { UserSession } from '../types';

export const deleteUserSession = ({
  userSessionKey,
}: {
  userSessionKey: string;
  currentSession: boolean;
}) => {
  return axios.delete(`/user-sessions/${userSessionKey}`);
};

type UseDeleteUserSessionOptions = {
  config?: MutationConfig<typeof deleteUserSession>;
};

export const useDeleteUserSession = ({ config }: UseDeleteUserSessionOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedUserSession) => {
      await queryClient.cancelQueries([
        'search-user-sessions',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousUserSessions = queryClient.getQueryData<PaginationResponse<UserSession>>([
        'search-user-sessions',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-user-sessions',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousUserSessions?.data?.filter(
          (userSession) => userSession.key !== deletedUserSession.userSessionKey
        )
      );

      return { previousUserSessions };
    },
    onError: (_, __, context: any) => {
      if (context?.previousUserSessions) {
        queryClient.setQueryData(
          [
            'search-user-sessions',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousUserSessions
        );
      }
    },
    onSuccess: async (_, variables) => {
      if (variables.currentSession) {
        window.location.href = '/app';
        return;
      }

      await queryClient.invalidateQueries([
        'search-user-sessions',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('User Session Deleted');
    },
    ...config,
    mutationFn: deleteUserSession,
  });
};
