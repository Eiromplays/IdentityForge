import { Navigate, Route, Routes } from 'react-router-dom';

import { RoleInfo } from './RoleInfo';
import { Roles } from './Roles';

export const RolesRoutes = () => {
  return (
    <Routes>
      <Route path="" element={<Roles />} />
      <Route path=":id" element={<RoleInfo />} />
      <Route path="*" element={<Navigate to="." />} />
    </Routes>
  );
};
