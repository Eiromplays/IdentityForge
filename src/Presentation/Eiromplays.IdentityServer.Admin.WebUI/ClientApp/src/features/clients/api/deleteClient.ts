import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type DeleteClientDTO = {
  clientId: string;
};

export const deleteClient = ({ clientId }: DeleteClientDTO) => {
  return axios.delete(`/clients/${clientId}`);
};

type UseDeleteClientOptions = {
  config?: MutationConfig<typeof deleteClient>;
};

export const useDeleteClient = ({ config }: UseDeleteClientOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      toast.success('Client deleted');
    },
    ...config,
    mutationFn: deleteClient,
  });
};
