import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { UserSession } from '../types';

export const deleteBffUserSession = ({ userSessionKey }: { userSessionKey: string }) => {
  return axios.delete(`/user-sessions/${userSessionKey}`);
};

type UseDeleteBffUserSessionOptions = {
  config?: MutationConfig<typeof deleteBffUserSession>;
};

export const useDeleteBffUserSession = ({ config }: UseDeleteBffUserSessionOptions = {}) => {
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
    onSuccess: async () => {
      await queryClient.invalidateQueries('bff-sessions');
      toast.success('Bff User Session Deleted');
      window.location.href = '/';
    },
    ...config,
    mutationFn: deleteBffUserSession,
  });
};
