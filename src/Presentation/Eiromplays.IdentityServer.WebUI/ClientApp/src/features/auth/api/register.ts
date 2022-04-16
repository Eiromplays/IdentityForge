import { axios } from '@/lib/axios';

export type RegisterCredentialsDTO = {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
};

export type RegisterResponse = {
  message: string;
};

export const registerWithEmailAndPassword = (
  data: RegisterCredentialsDTO
): Promise<RegisterResponse> => {
  return axios.post('/users/self-register', data);
};
