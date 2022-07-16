import { useSearch } from '@tanstack/react-location';
import { axios, MutationConfig, PaginationResponse, queryClient } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { LocationGenerics } from '@/App';

import { PersistedGrant } from '../types';

export const deletePersistedGrant = ({ persistedGrantKey }: { persistedGrantKey: string }) => {
  return axios.delete(`/persisted-grants/${persistedGrantKey}`);
};

type UseDeletePersistedGrantOptions = {
  config?: MutationConfig<typeof deletePersistedGrant>;
};

export const useDeletePersistedGrant = ({ config }: UseDeletePersistedGrantOptions = {}) => {
  const { pagination } = useSearch<LocationGenerics>();

  return useMutation({
    onMutate: async (deletedPersistedGrant) => {
      await queryClient.cancelQueries(['search-persisted-grants']);

      const previousPersistedGrants = queryClient.getQueryData<PaginationResponse<PersistedGrant>>([
        'search-persisted-grants',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);

      queryClient.setQueryData(
        ['search-persisted-grants', pagination?.index ?? 1, pagination?.size ?? 10],
        previousPersistedGrants?.data?.filter(
          (persistedGrant) => persistedGrant.key !== deletedPersistedGrant.persistedGrantKey
        )
      );

      return { previousPersistedGrants };
    },
    onError: (_, __, context: any) => {
      if (context?.previousPersistedGrants) {
        queryClient.setQueryData(
          ['search-persisted-grants', pagination?.index ?? 1, pagination?.size ?? 10],
          context.previousPersistedGrants
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-persisted-grants',
        pagination?.index ?? 1,
        pagination?.size ?? 10,
      ]);
      toast.success('Persisted Grant deleted');
    },
    ...config,
    mutationFn: deletePersistedGrant,
  });
};
