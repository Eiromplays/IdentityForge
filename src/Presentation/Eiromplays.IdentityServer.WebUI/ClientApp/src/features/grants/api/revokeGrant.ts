import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

import { GrantsViewModel } from '../types';

export const revokeGrant = ({ clientId }: { clientId: string }) => {
  return axios.delete(`${identityServerUrl}/grants/revoke/${clientId}`);
};

type UseRevokeGrantOptions = {
  config?: MutationConfig<typeof revokeGrant>;
};

export const useRevokeGrant = ({ config }: UseRevokeGrantOptions = {}) => {
  return useMutation({
    onMutate: async (revokedGrant) => {
      await queryClient.cancelQueries('grants');

      const previousGrants = queryClient.getQueryData<GrantsViewModel>('grants');

      queryClient.setQueryData(
        'grants',
        previousGrants?.grants?.filter((grant) => grant.clientId !== revokedGrant.clientId)
      );

      return { previousGrants };
    },
    onError: (_, __, context: any) => {
      if (context?.previousDiscussions) {
        queryClient.setQueryData('grants', context.previousDiscussions);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries('grants');
      toast.success('Grant Revoked');
    },
    ...config,
    mutationFn: revokeGrant,
  });
};
