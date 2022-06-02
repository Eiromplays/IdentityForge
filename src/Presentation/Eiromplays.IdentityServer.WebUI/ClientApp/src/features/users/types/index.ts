export type EnableAuthenticatorViewModel = {
  code: string;
  sharedKey: string;
  authenticatorUri: string;
};

export type TwoFactorAuthenticationViewModel = {
  hasAuthenticator: boolean;
  recoveryCodesLeft: number;
  is2FaEnabled: boolean;
  isMachineRemembered: boolean;
};
