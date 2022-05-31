import { MutationConfig, axios, MessageResponse } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export type CreateIdentityResourceDTO = {
  data: {
    name: string;
    displayName: string;
    description: string;
    showInDiscoveryDocument: boolean;
    emphasize: boolean;
    enabled: boolean;
    required: boolean;
    nonEditable: boolean;
  };
};

export const createIdentityResource = async ({
  data,
}: CreateIdentityResourceDTO): Promise<MessageResponse> => {
  return axios.post(`/identity-resources`, data);
};

type UseCreateIdentityResourceOptions = {
  config?: MutationConfig<typeof createIdentityResource>;
};

export const useCreateIdentityResource = ({ config }: UseCreateIdentityResourceOptions = {}) => {
  return useMutation({
    onSuccess: async (response) => {
      toast.success('IdentityResource Created');
      toast.success(response.message);
    },
    onError: (error) => {
      toast.error('Failed to create IdentityResource');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: createIdentityResource,
  });
};
