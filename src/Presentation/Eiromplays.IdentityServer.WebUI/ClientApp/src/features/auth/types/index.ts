export type AuthUser = {
  id: string;
  username: string;
  profilePicture: string;
  email: string;
  roles: string[];
  logoutUrl: string;
};
