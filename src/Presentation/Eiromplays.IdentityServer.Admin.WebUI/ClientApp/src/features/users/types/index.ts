export type User = {
  id: string;
  userName: string;
  displayName: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
  gravatarEmail?: string;
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
  twoFactorEnabled: boolean;
  lockoutEnabled: boolean;
  isActive: boolean;
  profilePicture: string;
  createdOn: number;
  lastModifiedOn: number;
};

export type UserRole = {
  id: string;
  roleId: string;
  roleName: string;
  description: string;
  enabled: boolean;
};

export type UserClaim = {
  type: string;
  value: string;
  valueType: string;
  issuer: string;
  createdOn: number;
  lastModifiedOn: number;
};

export type UserProvider = {
  loginProvider: string;
  providerKey: string;
  providerDisplayName: string;
};
