import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig, queryClient } from '@/lib/react-query';

export type DownloadPersonalDataDTO = {
  userId: string;
};

export const downloadPersonalData = ({ userId }: DownloadPersonalDataDTO): Promise<any> => {
  return axios.get(`/user-personal-data/${userId}`);
};

type UsePersonalDataOptions = {
  config?: MutationConfig<typeof downloadPersonalData>;
};

export const usePersonalData = ({ config }: UsePersonalDataOptions = {}) => {
  const downloadPersonalDataMutation = useMutation({
    onError: (_, __, context: any) => {
      if (context?.previousUsers) {
        queryClient.setQueryData('personal-data', context.previousUsers);
      }
    },
    onSuccess: (data) => {
      // Probably not the best way to download files using javascript, but it works :)
      const url = window.URL.createObjectURL(
        new Blob([JSON.stringify(data, undefined, '\t')], { type: 'text/json' })
      );

      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `PersonalData.json`);
      document.body.appendChild(link);
      link.click();

      // Clean up and remove the link
      link.parentNode?.removeChild(link);
      toast.success('Downloaded Personal Data');
    },
    ...config,
    mutationFn: downloadPersonalData,
  });

  return { downloadPersonalDataMutation };
};
