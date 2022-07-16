import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';
import { ServerSideSession } from '@/features/sessions';

export const deleteServerSideSession = ({
  serverSideSessionKey,
}: {
  currentSession: boolean;
  serverSideSessionKey: string;
}) => {
  return axios.delete(`/server-side-sessions/${serverSideSessionKey}`);
};

type UseDeleteServerSideSessionOptions = {
  config?: MutationConfig<typeof deleteServerSideSession>;
};

export const useDeleteServerSideSession = ({ config }: UseDeleteServerSideSessionOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedServerSideSession) => {
      await queryClient.cancelQueries(['search-server-side-sessions']);

      const previousServerSideSessions = queryClient.getQueryData<
        PaginationResponse<ServerSideSession>
      >(['search-server-side-sessions', pagination?.index ?? 1, pagination?.size ?? 10]);

      queryClient.setQueryData(
        ['search-server-side-sessions', pagination?.index ?? 1, pagination?.size ?? 10],
        previousServerSideSessions?.data?.filter(
          (serverSideSession) =>
            serverSideSession.key !== deletedServerSideSession.serverSideSessionKey
        )
      );

      return { previousServerSideSessions };
    },
    onError: (_, __, context: any) => {
      if (context?.previousServerSideSessions) {
        queryClient.setQueryData(
          ['search-server-side-sessions', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousServerSideSessions
        );
      }
    },
    onSuccess: async (_, variables) => {
      if (variables.currentSession) {
        window.location.href = '/app';
        return;
      }

      await queryClient.invalidateQueries([
        'search-server-side-sessions',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('Server-side Session Deleted');
    },
    ...config,
    mutationFn: deleteServerSideSession,
  });
};
