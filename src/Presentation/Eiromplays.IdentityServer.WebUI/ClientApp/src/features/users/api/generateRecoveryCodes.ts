import { axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

export const generateRecoveryCodes = (): Promise<string[]> => {
  return axios.post(`https://localhost:7001/account/GenerateRecoveryCodes`);
};

type UseGenerateRecoveryCodesOptions = {
  config?: MutationConfig<typeof generateRecoveryCodes>;
};

export const useGenerateRecoveryCodes = ({ config }: UseGenerateRecoveryCodesOptions = {}) => {
  return useMutation({
    onSuccess: () => {
      toast.success('Successfully reset authentication app key.');
    },
    ...config,
    mutationFn: generateRecoveryCodes,
  });
};
