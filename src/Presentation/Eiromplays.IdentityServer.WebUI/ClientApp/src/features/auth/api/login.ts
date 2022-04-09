import { axios } from '@/lib/axios';

export type LoginCredentialsDTO = {
  login: string;
  password: string;
  returnUrl?: string;
};

export const loginWithEmailAndPassword = (data: LoginCredentialsDTO): Promise<any> => {
  return axios.post('https://localhost:7001/spa/Login', data, {
    withCredentials: true,
  });
};
