import { axios, MutationConfig, queryClient, useAuth } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { UserSession } from '../types';

export const deleteBffUserSession = ({
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  currentSession,
  userSessionKey,
}: {
  currentSession: boolean;
  userSessionKey: string;
}) => {
  return axios.delete(`/user-sessions/${userSessionKey}`);
};

type UseDeleteBffUserSessionOptions = {
  config?: MutationConfig<typeof deleteBffUserSession>;
};

export const useDeleteBffUserSession = ({ config }: UseDeleteBffUserSessionOptions = {}) => {
  const { logout } = useAuth();

  return useMutation({
    onMutate: async (deletedUserSession) => {
      await queryClient.cancelQueries('bff-sessions');

      const previousUserSessions = queryClient.getQueryData<UserSession[]>('bff-sessions');

      queryClient.setQueryData(
        'bff-sessions',
        previousUserSessions?.filter(
          (userSession) => userSession.key !== deletedUserSession.userSessionKey
        )
      );

      return { previousUserSessions };
    },
    onError: (_, __, context: any) => {
      if (context?.previousUserSessions) {
        queryClient.setQueryData('bff-sessions', context.previousUserSessions);
      }
    },
    onSuccess: async (_, variables) => {
      if (variables.currentSession) {
        await logout();
        window.location.href = '/';
        return;
      }
      await queryClient.invalidateQueries('bff-sessions');
      toast.success('Bff User Session Deleted');
    },
    ...config,
    mutationFn: deleteBffUserSession,
  });
};
