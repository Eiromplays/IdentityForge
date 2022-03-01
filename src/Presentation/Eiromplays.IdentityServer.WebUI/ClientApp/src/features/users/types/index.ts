import { BaseEntity } from '@/types';

export type User = {
  id: string;
  userName: string;
  profilePicture: string;
  email: string;
  role: 'ADMIN' | 'USER';
} & BaseEntity;
