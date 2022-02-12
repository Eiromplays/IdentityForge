import Axios from 'axios';
import { toast } from 'react-toastify';

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
    const message = error.response?.data?.message || error.message;
    toast.error(message);

    return Promise.reject(error);
  }
);
