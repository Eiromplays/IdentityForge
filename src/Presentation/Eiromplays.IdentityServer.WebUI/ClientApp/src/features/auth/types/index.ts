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

export type LoginConsentResponse = {
  error: string;
  signInResult: SignInResult;
  validReturnUrl: string;
};

export type SignInResult = {
  succeeded: boolean;
  isLockedOut: boolean;
  isNotAllowed: boolean;
  RequiresTwoFactor: boolean;
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
