import { ClaimDto } from '@/types';

export type Role = {
  id: string;
  name: string;
  description: string;
};

export type RoleClaim = {
  id: number;
  createdOn: number;
  lastModifiedOn: number;
  claim: ClaimDto;
};
