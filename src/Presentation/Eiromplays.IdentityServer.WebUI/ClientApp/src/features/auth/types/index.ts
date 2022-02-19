export type UserData = {
  id: string;
  userName: string;
  profilePicture: string;
  email: string;
};

export type UserSessionInfo = {
  logoutUrl: string;
  username?: string;
};

export type AuthUser = {
  data: UserData;
  sessionInfo: UserSessionInfo;
};
