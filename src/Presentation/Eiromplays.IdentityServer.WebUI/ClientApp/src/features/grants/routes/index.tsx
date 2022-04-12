import { Navigate, Route, Routes } from 'react-router-dom';

import { Grant } from './Grant';
import { Grants } from './Grants';

export const GrantsRoutes = () => {
  return (
    <Routes>
      <Route path="" element={<Grants />} />
      <Route path=":clientId" element={<Grant />} />
      <Route path="*" element={<Navigate to="." />} />
    </Routes>
  );
};
