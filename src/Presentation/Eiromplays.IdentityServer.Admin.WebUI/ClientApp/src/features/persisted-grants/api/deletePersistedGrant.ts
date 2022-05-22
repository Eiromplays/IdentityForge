import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { PersistedGrant } from '../types';

export const deletePersistedGrant = ({ persistedGrantKey }: { persistedGrantKey: string }) => {
  return axios.delete(`/persisted-grants/${persistedGrantKey}`);
};

type UseDeletePersistedGrantOptions = {
  config?: MutationConfig<typeof deletePersistedGrant>;
};

export const useDeletePersistedGrant = ({ config }: UseDeletePersistedGrantOptions = {}) => {
  return useMutation({
    onMutate: async (deletedPersistedGrant) => {
      await queryClient.cancelQueries('persisted-grants');

      const previousPersistedGrants =
        queryClient.getQueryData<PersistedGrant[]>('persisted-grants');

      queryClient.setQueryData(
        'persisted-grants',
        previousPersistedGrants?.filter(
          (persistedGrant) => persistedGrant.key !== deletedPersistedGrant.persistedGrantKey
        )
      );

      return { previousPersistedGrants };
    },
    onError: (_, __, context: any) => {
      if (context?.previousPersistedGrants) {
        queryClient.setQueryData('persisted-grants', context.previousPersistedGrants);
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries('persisted-grants');
      toast.success('Persisted Grant Deleted');
    },
    ...config,
    mutationFn: deletePersistedGrant,
  });
};
