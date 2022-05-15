import { useAuth, axios, MutationConfig } from 'eiromplays-ui';
import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { ConsentInputModel, ProcessConsentResult } from '../types';

type ConsentDto = {
  data: ConsentInputModel;
};

export const consent = async ({ data }: ConsentDto): Promise<ProcessConsentResult> => {
  return axios.post('https://localhost:7001/consent', data);
};

type UseConsentOptions = {
  config?: MutationConfig<typeof consent>;
};

export const useConsent = ({ config }: UseConsentOptions = {}) => {
  const { refetchUser } = useAuth();

  return useMutation({
    onSuccess: async (response) => {
      toast.success('Consent granted');

      if (response.isRedirect && response.redirectUri) window.location.href = response.redirectUri;

      await refetchUser();
    },
    onError: (error) => {
      toast.error('Failed to consent');
      toast.error(error.response?.data);
    },
    ...config,
    mutationFn: consent,
  });
};
