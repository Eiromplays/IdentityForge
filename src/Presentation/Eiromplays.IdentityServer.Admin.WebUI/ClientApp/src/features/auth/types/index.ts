export type AuthUser = {
  id: string;
  sessionId: string;
  username: string;
  firstName: string;
  lastName: string;
  profilePicture: string;
  email: string;
  gravatarEmail?: string;
  roles: string[];
  logoutUrl: string;
  updated_at: string;
  created_at: string;
};
