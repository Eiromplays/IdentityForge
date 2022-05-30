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
 consentLifetime?: number;
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
 userSsoLifetime?: number;
 userCodeType: string;
 deviceCodeLifetime: number;
 cibaLifetime?: number;
 pollingInterval?: number;
 coordinateLifetimeWithUserSession?: boolean;
 created: number;
 updated: number;
 lastAccessed: number;
 nonEditable: boolean;
};

export type ClientClaim = {
  id: number;
  type: string;
  value: string;
};

export type ClientSecret = {
  id: number;
  value: string;
  description: string;
  expiration: number;
  type: string;
  created: number;
};

export type ClientProperty = {
  id: number;
  key: string;
  value: string;
};

export type ClientGrantType = {
  id: number;
  grantType: string;
}

export type ClientRedirectUri = {
  id: number;
  redirectUri: string;
}

export type ClientPostLogoutRedirectUri = {
  id: number;
  postLogoutRedirectUri: string;
}

export type ClientScope = {
  id: number;
  scope: string;
};

export type ClientIdPRestriction = {
  id: number;
  provider: string;
};

export type ClientCorsOrigin = {
  id: number;
  origin: string;
};

export type SearchClientDTO = PaginationFilter;
