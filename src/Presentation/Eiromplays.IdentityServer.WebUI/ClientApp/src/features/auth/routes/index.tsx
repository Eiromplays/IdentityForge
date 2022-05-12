import { Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { ConfirmedEmail } from './ConfirmedEmail';
import { ExternalLoginConfirmation } from './ExternalLoginConfirmation';
import { ForgotPassword } from './ForgotPassword';
import { Lockout } from './Lockout';
import { Login } from './Login';
import { Login2fa } from './Login2fa';
import { Logout } from './Logout';
import { NotAllowed } from './NotAllowed';
import { Register } from './Register';

export const AuthRoutes: Route<LocationGenerics> = {
  path: 'auth',
  children: [
    { path: 'register', element: <Register /> },
    { path: 'login', element: <Login /> },
    { path: 'login/:rememberMe/:returnUrl', element: <Login2fa /> },
    { path: 'login/:rememberMe', element: <Login2fa /> },
    {
      path: 'external-login-confirmation/:email/:userName/:loginProvider',
      element: <ExternalLoginConfirmation />,
    },
    { path: 'not-allowed', element: <NotAllowed /> },
    { path: 'confirmed-email', element: <ConfirmedEmail /> },
    { path: 'forgot-password', element: <ForgotPassword /> },
    { path: 'logout', element: <Logout /> },
    { path: 'lockout', element: <Lockout /> },
  ],
};
