import { Navigate, Route, Routes } from 'react-router-dom';

import { UserSession } from './UserSession';
import { UserSessions } from './UserSessions';

export const UserSessionsRoutes = () => {
  return (
    <Routes>
      <Route path="" element={<UserSessions />} />
      <Route path=":key" element={<UserSession />} />
      <Route path="*" element={<Navigate to="." />} />
    </Routes>
  );
};
