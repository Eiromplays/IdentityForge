import { BaseEntity } from '@/types';

export type User = {
  id: string;
  username: string;
  profilePicture: string;
  email: string;
  roles: string[];
} & BaseEntity;
