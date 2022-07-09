import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export const deleteServerSideSession = ({
  serverSideSessionKey,
}: {
  serverSideSessionKey: string;
}) => {
  return axios.delete(`${identityServerUrl}/api/v1/manage/user-sessions/${serverSideSessionKey}`);
};

type UseDeleteServerSideSessionOptions = {
  config?: MutationConfig<typeof deleteServerSideSession>;
};

export const useDeleteServerSideSession = ({ config }: UseDeleteServerSideSessionOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      await queryClient.invalidateQueries('server-side-sessions');
      toast.success('Server-side Session Deleted');
      window.location.href = '/';
    },
    ...config,
    mutationFn: deleteServerSideSession,
  });
};
