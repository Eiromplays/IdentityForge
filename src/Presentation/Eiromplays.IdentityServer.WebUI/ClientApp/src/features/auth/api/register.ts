import { axios } from '@/lib/axios';

export type RegisterCredentialsDTO = {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
};

export const registerWithEmailAndPassword = (data: RegisterCredentialsDTO): Promise<any> => {
  return axios.post('/auth/register', data);
};
