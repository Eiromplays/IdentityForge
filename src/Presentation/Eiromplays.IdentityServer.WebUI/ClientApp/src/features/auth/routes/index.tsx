import { Route } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { ResetPassword } from '../routes/ResetPassword';

import { ConfirmedEmail } from './ConfirmedEmail';
import { ConfirmedPhoneNumber } from './ConfirmedPhoneNumber';
import { ConfirmEmail } from './ConfirmEmail';
import { ConfirmPhoneNumber } from './ConfirmPhoneNumber';
import { ExternalLoginConfirmation } from './ExternalLoginConfirmation';
import { ForgotPassword } from './ForgotPassword';
import { Lockout } from './Lockout';
import { Login } from './Login';
import { Login2fa } from './Login2fa';
import { Logout } from './Logout';
import { NotAllowed } from './NotAllowed';
import { Register } from './Register';
import { ResendEmailConfirmation } from './ResendEmailConfirmation';
import { ResendPhoneNumberConfirmation } from './ResendPhoneNumberConfirmation';

export const AuthRoutes: Route<LocationGenerics> = {
  path: 'auth',
  children: [
    { path: 'register', element: <Register /> },
    { path: 'login', element: <Login /> },
    { path: 'login2fa', element: <Login2fa /> },
    {
      path: 'external-login-confirmation',
      element: <ExternalLoginConfirmation />,
    },
    { path: 'not-allowed', element: <NotAllowed /> },
    { path: 'confirmed-email', element: <ConfirmedEmail /> },
    { path: 'confirm-email', element: <ConfirmEmail /> },
    { path: 'confirmed-phone-number', element: <ConfirmedPhoneNumber /> },
    { path: 'verify-phone-number', element: <ConfirmPhoneNumber /> },
    { path: 'forgot-password', element: <ForgotPassword /> },
    { path: 'reset-password', element: <ResetPassword /> },
    { path: 'logout', element: <Logout /> },
    { path: 'lockout', element: <Lockout /> },
    { path: 'resend-email-confirmation', element: <ResendEmailConfirmation /> },
    { path: 'resend-phone-number-confirmation', element: <ResendPhoneNumberConfirmation /> },
  ],
};
