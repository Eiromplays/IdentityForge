import { Route, Routes } from 'react-router-dom';

import { ExternalLoginConfirmation } from './ExternalLoginConfirmation';
import { Login } from './Login';
import { Login2fa } from './Login2fa';
import { NotAllowed } from './NotAllowed';
import { Register } from './Register';

export const AuthRoutes = () => {
  return (
    <Routes>
      <Route path="register" element={<Register />} />
      <Route path="login" element={<Login />} />
      <Route path="login2fa/:rememberMe/:returnUrl" element={<Login2fa />} />
      <Route path="login2fa/:rememberMe" element={<Login2fa />} />
      <Route
        path="external-login-confirmation/:email/:userName/:loginProvider"
        element={<ExternalLoginConfirmation />}
      />
      <Route path="not-allowed" element={<NotAllowed />} />
    </Routes>
  );
};
