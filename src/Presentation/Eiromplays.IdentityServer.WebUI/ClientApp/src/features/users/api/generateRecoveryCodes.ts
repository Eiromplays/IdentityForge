import { useMutation } from '@tanstack/react-query';
import { axios, MutationConfig, queryClient } from 'eiromplays-ui';
import { toast } from 'react-toastify';

import { identityServerUrl } from '@/utils/envVariables';

export const generateRecoveryCodes = (): Promise<string[]> => {
  return axios.post(
    `${identityServerUrl}/api/v1/manage/two-factor-authentication/generate-recovery-codes`
  );
};

type UseGenerateRecoveryCodesOptions = {
  config?: MutationConfig<typeof generateRecoveryCodes>;
};

export const useGenerateRecoveryCodes = ({ config }: UseGenerateRecoveryCodesOptions = {}) => {
  return useMutation({
    onSuccess: async () => {
      await queryClient.invalidateQueries(['two-factor-authentication']);
      toast.success('Successfully reset authentication app key.');
    },
    ...config,
    mutationFn: generateRecoveryCodes,
  });
};
