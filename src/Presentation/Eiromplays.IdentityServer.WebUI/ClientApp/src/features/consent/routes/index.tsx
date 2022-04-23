import { Navigate, Route, Routes } from 'react-router-dom';

import { Consent } from './Consent';

export const ConsentRoutes = () => {
  return (
    <Routes>
      <Route path="" element={<Consent />} />
      <Route path="*" element={<Navigate to="." />} />
    </Routes>
  );
};
