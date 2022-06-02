import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type DeleteApiScopeDTO = {
  apiScopeId: number;
};

export const deleteApiScope = ({ apiScopeId }: DeleteApiScopeDTO) => {
  return axios.delete(`/api-scopes/${apiScopeId}`);
};

type UseDeleteApiScopeOptions = {
  config?: MutationConfig<typeof deleteApiScope>;
};

export const useDeleteApiScope = ({ config }: UseDeleteApiScopeOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      toast.success('ApiScope deleted');
    },
    ...config,
    mutationFn: deleteApiScope,
  });
};
