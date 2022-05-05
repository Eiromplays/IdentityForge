import { Navigate, Route, Routes } from 'react-router-dom';

import { PersistedGrant } from './PersistedGrant';
import { PersistedGrants } from './PersistedGrants';

export const PersistedGrantsRoutes = () => {
  return (
    <Routes>
      <Route path="" element={<PersistedGrants />} />
      <Route path=":key" element={<PersistedGrant />} />
      <Route path="*" element={<Navigate to="." />} />
    </Routes>
  );
};
