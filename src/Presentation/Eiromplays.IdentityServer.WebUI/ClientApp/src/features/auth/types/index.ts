export type AuthUser = {
  id: string;
  username: string;
  profilePicture: string;
  email: string;
  gravatarEmail?: string;
  roles: string[];
  logoutUrl: string;
  updated_at: Date;
  created_at: Date;
};
