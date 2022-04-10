export type AuthUser = {
  id: string;
  username: string;
  profilePicture: string;
  email: string;
  gravatarEmail?: string;
  roles: string[];
  logoutUrl: string;
  updated_at: Date;
  created_at: Date;
};

export type LoginConsentResponse = {
  error: string;
  validReturnUrl: string;
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
