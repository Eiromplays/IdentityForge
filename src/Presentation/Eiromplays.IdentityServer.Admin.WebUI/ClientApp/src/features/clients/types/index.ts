import { PaginationFilter } from 'eiromplays-ui';

export type Client = {
  id: number;
  enabled: boolean;
  clientId: string;
  protocolType: string;
  clientSecrets: ClientSecret[];
  requireClientSecret: boolean;
  clientName: string;
  description: string;
  clientUri: string;
  logoUri: string;
  requireConsent: boolean;
  allowRememberConsent: boolean;
  alwaysIncludeUserClaimsInIdToken: boolean;
  allowedGrantTypes: ClientGrantType[];
  requirePkce: boolean;
  allowPlainTextPkce: boolean;
  requireRequestObject: boolean;
  allowAccessTokensViaBrowser: boolean;
  redirectUris: ClientRedirectUri[];
  postLogoutRedirectUris: ClientPostLogoutRedirectUri[];
  frontChannelLogoutUri: string;
  frontChannelLogoutSessionRequired: boolean;
  backChannelLogoutUri: string;
  backChannelLogoutSessionRequired: boolean;
  allowOfflineAccess: boolean;
  allowedScopes: ClientScope[];
  identityTokenLifetime: number;
  allowedIdentityTokenSigningAlgorithms: string;
  accessTokenLifetime: number;
  authorizationCodeLifetime: number;
  consentLifetime: number;
  absoluteRefreshTokenLifetime: number;
  slidingRefreshTokenLifetime: number;
  refreshTokenUsage: number;
  updateAccessTokenClaimsOnRefresh: boolean;
  refreshTokenExpiration: number;
  accessTokenType: number;
  enableLocalLogin: boolean;
  identityProviderRestrictions: ClientIdPRestriction[];
  includeJwtId: boolean;
  claims: ClientClaim[];
  alwaysSendClientClaims: boolean;
  clientClaimsPrefix: string;
  pairWiseSubjectSalt: string;
  allowedCorsOrigins: ClientCorsOrigin[];
  properties: ClientProperty[];
  userSsoLifetime: number;
  userCodeType: string;
  deviceCodeLifetime: number;
  cibaLifetime: number;
  pollingInterval: number;
  coordinateLifetimeWithUserSession: boolean;
  created: string;
  updated: string;
  lastAccessed: string;
  nonEditable: boolean;
};

export type Secret = {
  id: number;
  description: string;
  value: string;
  expiration: string;
  type: string;
  created: string;
};

export type ClientSecret = Secret & {
  clientId: number;
  client: Client;
};

export type ClientGrantType = {
  id: number;
  grantType: string;
  clientId: number;
  client: Client;
};

export type ClientRedirectUri = {
  id: number;
  redirectUri: string;
  clientId: number;
  client: Client;
};

export type ClientPostLogoutRedirectUri = {
  id: number;
  postLogoutRedirectUri: string;
  clientId: number;
  client: Client;
};

export type ClientScope = {
  id: number;
  scope: string;
  clientId: number;
  client: Client;
};

export type ClientIdPRestriction = {
  id: number;
  provider: string;
  clientId: number;
  client: Client;
};

export type ClientClaim = {
  id: number;
  type: string;
  value: string;
  clientId: number;
  client: Client;
};

export type ClientCorsOrigin = {
  id: number;
  origin: string;
  clientId: number;
  client: Client;
};

export type Property = {
  id: number;
  key: string;
  value: string;
};

export type ClientProperty = Property & {
  clientId: number;
  client: Client;
};

export type SearchClientDTO = PaginationFilter;
