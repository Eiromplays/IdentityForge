export type UserData = {
  id: string;
  userName: string;
  profilePicture: string;
  email: string;
  role: 'ADMIN' | 'USER';
};

export type UserSessionInfo = {
  id?: string;
  username?: string;
  logoutUrl: string;
};

export type AuthUser = {
  data: UserData;
  sessionInfo: UserSessionInfo;
};

export type UserResponse = {
  jwt: string;
  user: AuthUser;
};
