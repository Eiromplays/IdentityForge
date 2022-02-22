export type UserData = {
  id: string;
  userName: string;
  profilePicture: string;
  email: string;
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
