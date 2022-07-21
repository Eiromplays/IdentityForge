import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

import { Grant } from '../types';

export const revokeGrant = ({ clientId }: { clientId: string }) => {
  return axios.delete(`${identityServerUrl}/api/v1/grants/${clientId}`);
};

type UseRevokeGrantOptions = {
  config?: MutationConfig<typeof revokeGrant>;
};

export const useRevokeGrant = ({ config }: UseRevokeGrantOptions = {}) => {
  return useMutation({
    onMutate: async (revokedGrant) => {
      await queryClient.cancelQueries(['grants']);

      const previousGrants = queryClient.getQueryData<Grant[]>(['grants']);

      queryClient.setQueryData(
        ['grants'],
        previousGrants?.filter((grant) => grant.clientId !== revokedGrant.clientId)
      );

      return { previousGrants };
    },
    onError: (_, __, context: any) => {
      if (context?.previousGrants) {
        queryClient.setQueryData(['grants'], context.previousGrants);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries(['grants']);
      toast.success('Grant Revoked');
    },
    ...config,
    mutationFn: revokeGrant,
  });
};
