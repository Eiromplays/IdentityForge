import { useAuth } from 'eiromplays-ui';
import * as React from 'react';

export enum ROLES {
  ADMINISTRATOR = 'ADMINISTRATOR',
  USER = 'USER',
}

type RoleTypes = keyof typeof ROLES;

export const POLICIES = {};

export const useAuthorization = () => {
  const { user } = useAuth();

  const checkAccess = React.useCallback(
    ({ allowedRoles }: { allowedRoles: RoleTypes[] }) => {
      if (allowedRoles && allowedRoles.length > 0) {
        return allowedRoles?.every((role) => user?.roles.includes(role.toLowerCase()));
      }

      return true;
    },
    [user?.roles]
  );

  return { checkAccess, role: user?.roles };
};

type AuthorizationProps = {
  forbiddenFallback?: React.ReactNode;
  children: React.ReactNode;
} & (
  | {
      allowedRoles: RoleTypes[];
      policyCheck?: never;
    }
  | {
      allowedRoles?: never;
      policyCheck: boolean;
    }
);

export const Authorization = ({
  policyCheck,
  allowedRoles,
  forbiddenFallback = null,
  children,
}: AuthorizationProps) => {
  const { checkAccess } = useAuthorization();

  let canAccess = false;

  if (allowedRoles) {
    canAccess = checkAccess({ allowedRoles });
  }

  if (typeof policyCheck !== 'undefined') {
    canAccess = policyCheck;
  }

  return <>{canAccess ? children : forbiddenFallback}</>;
};
