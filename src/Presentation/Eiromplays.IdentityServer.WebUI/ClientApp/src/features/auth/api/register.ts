import { axios } from '@/lib/axios';

export type RegisterCredentialsDTO = {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
};

export const registerWithEmailAndPassword = (data: RegisterCredentialsDTO): Promise<any> => {
  return axios.post('https://localhost:7001/spa/register', data);
};
