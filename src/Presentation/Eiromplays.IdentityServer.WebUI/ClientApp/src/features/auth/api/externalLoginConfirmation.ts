import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

type ExternalLoginConfirmationViewModel = {
  userName: string;
  email: string;
};

export const externalLoginConfirmation = ({
  returnUrl,
  data,
}: {
  data: ExternalLoginConfirmationViewModel;
  returnUrl?: string;
}) => {
  return axios.post(
    `https://localhost:7001/spa/externalLoginConfirmation?returnUrl=${returnUrl}`,
    data
  );
};

type UseRevokeGrantOptions = {
  config?: MutationConfig<typeof externalLoginConfirmation>;
};

export const useExternalLoginConfirmation = ({ config }: UseRevokeGrantOptions = {}) => {
  return useMutation({
    onSuccess: () => {
      toast.success('Successfully created account');
    },
    ...config,
    mutationFn: externalLoginConfirmation,
  });
};
