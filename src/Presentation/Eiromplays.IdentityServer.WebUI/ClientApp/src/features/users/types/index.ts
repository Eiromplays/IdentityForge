export type EnableAuthenticatorRequest = {
  provider: string;
  code: string;
};

export type TwoFactorAuthenticationViewModel = {
  validProviders: string[];
  hasAuthenticator: boolean;
  recoveryCodesLeft: number;
  is2FaEnabled: boolean;
  isMachineRemembered: boolean;
};

export type GetEnableAuthenticatorResponse = {
  validProviders: string[];
  sharedKey: string;
  authenticatorUri: string;
};
