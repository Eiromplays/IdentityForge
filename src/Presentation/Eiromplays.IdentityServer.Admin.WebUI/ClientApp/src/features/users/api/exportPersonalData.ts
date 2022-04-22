import { useMutation } from 'react-query';
import { toast } from 'react-toastify';

import { axios } from '@/lib/axios';
import { MutationConfig } from '@/lib/react-query';

export const exportPersonalData = (): Promise<any> => {
  return axios.get(`/personal/export-personal-data`, {
    headers: {
      'Content-Disposition': 'attachment; filename=PersonalData.csv',
      'Content-Type': 'application/octet-stream',
    },
    responseType: 'arraybuffer',
  });
};

type UseExportPersonalDataOptions = {
  config?: MutationConfig<typeof exportPersonalData>;
};

export const useExportPersonalData = ({ config }: UseExportPersonalDataOptions = {}) => {
  return useMutation({
    onSuccess: (data) => {
      // Probably not the best way to download files using javascript, but it works :)
      const url = window.URL.createObjectURL(new Blob([data]));

      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `PersonalData.csv`);
      document.body.appendChild(link);
      link.click();

      // Clean up and remove the link
      link.parentNode?.removeChild(link);
      toast.success('Exported Personal Data');
    },
    ...config,
    mutationFn: exportPersonalData,
  });
};
