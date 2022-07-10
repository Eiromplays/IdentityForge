export type AuthUser = {
  id: string;
  sessionId: string;
  username: string;
  firstName: string;
  lastName: string;
  profilePicture: string;
  email: string;
  email_verified: boolean;
  gravatarEmail?: string;
  phone_number: string;
  phone_number_verified: boolean;
  roles: string[];
  logoutUrl: string;
  updated_at: string;
  created_at: string;
};

export type LoginResponse = {
  error: string;
  signInResult: SignInResult;
  validReturnUrl: string;
  twoFactorReturnUrl?: string;
  message?: string;
};

export type SignInResult = {
  succeeded: boolean;
  isLockedOut: boolean;
  isNotAllowed: boolean;
  RequiresTwoFactor: boolean;
};

export type LoginViewModel = {
  allowRememberLogin: boolean;
  enableLocalLogin: boolean;
  externalProviders: ExternalProvider[];
  visibleExternalProviders: ExternalProvider[];
  isExternalLoginOnly: boolean;
  externalLoginScheme?: string;
};

export type ExternalProvider = {
  displayName: string;
  authenticationScheme: string;
};

// TODO: possibly find a better way to handle logout data
export type LoggedOutViewModel = {
  postLogoutRedirectUri: string;
  clientName: string;
  signOutIFrameUrl: string;
  automaticRedirectAfterSignOut: boolean;
  logoutId: string;
  triggerExternalSignout: boolean;
  externalAuthenticationScheme: string;
};

export type LogoutViewModel = {
  logoutId: string;
  showLogoutPrompt: boolean;
};

export type LogoutResponse = {
  logoutViewModel: LogoutViewModel;
  loggedOutViewModel: LoggedOutViewModel;
};
