import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
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
      await queryClient.cancelQueries(['search-user-sessions']);

      const previousUserSessions = queryClient.getQueryData<PaginationResponse<UserSession>>([
        'search-user-sessions',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-user-sessions', pagination?.index ?? 1, pagination?.size ?? 10],
        previousUserSessions?.data?.filter(
          (userSession) => userSession.key !== deletedUserSession.userSessionKey
        )
      );

      return { previousUserSessions };
    },
    onError: (_, __, context: any) => {
      if (context?.previousUserSessions) {
        queryClient.setQueryData(
          ['search-user-sessions', pagination?.index ?? 1, pagination?.size ?? 10],
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
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('User Session Deleted');
    },
    ...config,
    mutationFn: deleteUserSession,
  });
};
