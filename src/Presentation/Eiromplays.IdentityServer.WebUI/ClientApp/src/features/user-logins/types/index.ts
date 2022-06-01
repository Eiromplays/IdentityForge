export type UserLogin = {
  loginProvider: string;
  providerKey: string;
  providerDisplayName: string;
};

export type AuthenticationScheme = {
  name: string;
  displayName: string;
};

export type ExternalLoginsResponse = {
  currentLogins: UserLogin[];
  otherLogins: AuthenticationScheme[];
  showRemoveButton: boolean;
};
