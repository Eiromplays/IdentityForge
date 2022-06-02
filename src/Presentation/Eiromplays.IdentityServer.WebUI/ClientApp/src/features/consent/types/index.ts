export type ConsentViewModel = ConsentInputModel & {
  clientName: string;
  clientUrl: string;
  clientLogoUrl: string;
  allowRememberConsent: boolean;
  identityScopes: ScopeViewModel[];
  apiScopes: ScopeViewModel[];
};

export type ConsentInputModel = {
  button: string;
  scopesConsented: string[];
  rememberConsent: boolean;
  returnUrl?: string;
  description?: string;
};

export type ScopeViewModel = {
  value: string;
  displayName: string;
  description: string;
  emphasize: boolean;
  required: boolean;
  checked: boolean;
};

export type ProcessConsentResult = {
  isRedirect: boolean;
  redirectUri: string;
  client: any;
  showView: boolean;
  viewModel: ConsentViewModel;
  hasValidations: boolean;
  validationError: string;
};
