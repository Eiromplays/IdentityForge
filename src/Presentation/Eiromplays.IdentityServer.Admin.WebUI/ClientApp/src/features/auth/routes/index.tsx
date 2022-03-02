import { Route, Routes } from 'react-router-dom';

export const AuthRoutes = () => {
  return (
    <Routes>
      <Route path="register" element={<>Register</>} />
      <Route path="login" element={<>Login</>} />
    </Routes>
  );
};
