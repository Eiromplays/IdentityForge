export type User = {
  id: string;
  userName: string;
  firstName: string;
  lastName: string;
  profilePicture: string;
  email: string;
  gravatarEmail?: string;
  roles: string[];
  updated_at: number;
  created_at: number;
};
