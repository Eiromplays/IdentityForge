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
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);

      queryClient.setQueryData(
        [
          'search-persisted-grants',
          pagination?.index || defaultPageIndex,
          pagination?.size || defaultPageSize,
        ],
        previousPersistedGrants?.data?.filter(
          (persistedGrant) => persistedGrant.key !== deletedPersistedGrant.persistedGrantKey
        )
      );

      return { previousPersistedGrants };
    },
    onError: (_, __, context: any) => {
      if (context?.previousPersistedGrants) {
        queryClient.setQueryData(
          [
            'search-persisted-grants',
            pagination?.index || defaultPageIndex,
            pagination?.size || defaultPageSize,
          ],
          context.previousPersistedGrants
        );
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries([
        'search-persisted-grants',
        pagination?.index || defaultPageIndex,
        pagination?.size || defaultPageSize,
      ]);
      toast.success('Persisted Grant deleted');
    },
    ...config,
    mutationFn: deletePersistedGrant,
  });
};
