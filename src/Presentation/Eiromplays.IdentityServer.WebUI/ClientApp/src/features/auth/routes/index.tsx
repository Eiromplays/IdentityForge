import { Route, Routes } from 'react-router-dom';

export const AuthRoutes = () => {
  return (
    <Routes>
      <Route
        path="/"
        element={
          <>
            <h1>Test</h1>
          </>
        }
      />
    </Routes>
  );
};
