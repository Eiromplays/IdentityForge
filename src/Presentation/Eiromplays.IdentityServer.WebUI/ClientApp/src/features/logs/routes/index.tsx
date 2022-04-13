import { Navigate, Route, Routes } from 'react-router-dom';

import { Log } from './Log';
import { Logs } from './Logs';

export const LogsRoutes = () => {
  return (
    <Routes>
      <Route path="" element={<Logs />} />
      <Route path=":logId" element={<Log />} />
      <Route path="*" element={<Navigate to="." />} />
    </Routes>
  );
};
