import { Route, Routes } from 'react-router-dom';

import { Login } from './Login';
import { Login2fa } from './Login2fa';

export const AuthRoutes = () => {
  return (
    <Routes>
      <Route path="register" element={<>Register</>} />
      <Route path="login" element={<Login />} />
      <Route path="login2fa/:rememberMe/:returnUrl" element={<Login2fa />} />
      <Route path="login2fa/:rememberMe" element={<Login2fa />} />
    </Routes>
  );
};
