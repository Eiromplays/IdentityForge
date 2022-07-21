import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export const deleteServerSideSession = ({
  sessionId,
}: {
  currentSession: boolean;
  sessionId: string;
}) => {
  return axios.delete(`${identityServerUrl}/api/v1/manage/user-sessions/${sessionId}`);
};

type UseDeleteServerSideSessionOptions = {
  config?: MutationConfig<typeof deleteServerSideSession>;
};

export const useDeleteServerSideSession = ({ config }: UseDeleteServerSideSessionOptions = {}) => {
  return useMutation({
    onSuccess: async (_, variables) => {
      if (variables.currentSession) {
        window.location.href = '/';
        return;
      }
      await queryClient.invalidateQueries(['server-side-sessions']);
      toast.success('Server-side Session Deleted');
    },
    ...config,
    mutationFn: deleteServerSideSession,
  });
};
