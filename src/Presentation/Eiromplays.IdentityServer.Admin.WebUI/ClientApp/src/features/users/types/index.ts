export type User = {
  id: string;
  userName: string;
  firstName: string;
  lastName: string;
  profilePicture: string;
  phoneNumber: string;
  email: string;
  gravatarEmail?: string;
  roles: string[];
  updated_at: number;
  created_at: number;
};

export type UserRole = {
  id: string;
  roleId: string;
  roleName: string;
  description: string;
  enabled: boolean;
};
