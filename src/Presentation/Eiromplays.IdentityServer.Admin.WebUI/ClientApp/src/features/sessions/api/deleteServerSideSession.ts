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
      await queryClient.cancelQueries([
        'search-server-side-sessions',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      const previousServerSideSessions = queryClient.getQueryData<
        PaginationResponse<ServerSideSession>
      >([
        'search-server-side-sessions',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-server-side-sessions',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
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
          [
            'search-server-side-sessions',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
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
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('Server-side Session Deleted');
    },
    ...config,
    mutationFn: deleteServerSideSession,
  });
};
