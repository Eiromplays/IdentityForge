import Axios from 'axios';
import { toast } from 'react-toastify';

import { WhitelistAxiosError } from '@/types';

const Whitelists: WhitelistAxiosError[] = [{ status: 401, urls: ['/bff/user'], ignoreAll: false }];

export const axios = Axios.create({
  headers: {
    'X-CSRF': '1',
  },
});

axios.defaults.timeout = 30_000; // If you want to increase this, do it for a specific call, not the global app API.

axios.interceptors.response.use(
  (response) => {
    return response.data;
  },
  (error) => {
    const whitelist = Whitelists.find(
      (whitelist: WhitelistAxiosError) => whitelist.status === error.response.status
    );

    if (
      (whitelist &&
        whitelist.urls.includes(new URL(error.request.responseURL).pathname.replace(/\/$/, ''))) ||
      (whitelist && whitelist.ignoreAll)
    ) {
      return;
    }

    const messages =
      error.response?.data?.errors?.GeneralErrors ?? error.response?.data?.messages ?? [];

    if (messages.length <= 0) {
      toast.error(error.response?.data?.message || error.message);
      return Promise.reject(error);
    }
    messages.forEach((message: string) => toast.error(message));

    return Promise.reject(error);
  }
);
