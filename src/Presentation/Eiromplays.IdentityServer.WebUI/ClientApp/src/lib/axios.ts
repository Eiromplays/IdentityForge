import Axios from 'axios';
import { toast } from 'react-toastify';

import { WhitelistAxiosError } from '@/types';

const Whitelists: WhitelistAxiosError[] = [
  { status: 401, urls: ['/bff/user'], ignoreAll: false },
  { status: 400, urls: ['/spa/login'] },
];

export const axios = Axios.create({
  headers: {
    'X-CSRF': '1',
  },
  withCredentials: true,
});

axios.defaults.timeout = 30_000; // If you want to increase this, do it for a specific call, not the global app API.

axios.interceptors.response.use(
  (response) => {
    return response.data;
  },
  (error) => {
    const whitelists = Whitelists.filter(
      (whitelist: WhitelistAxiosError) => whitelist.status === error.response.status
    );

    const shouldWhitelist = whitelists.some(
      (whitelist) =>
        whitelist.urls.some(
          (url) =>
            url.toLowerCase() ===
            new URL(error.request.responseURL).pathname.replace(/\/$/, '').toLowerCase()
        ) || whitelist.ignoreAll
    );

    if (shouldWhitelist) {
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
