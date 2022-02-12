import Typography from '@mui/material/Typography';
import { Route, Routes } from 'react-router-dom';

export const AuthRoutes = () => {
  return (
    <Routes>
      <Route
        path="/test"
        element={
          <>
            <div>
              <Typography variant="h1" component="h2">
                h1. Heading
              </Typography>
            </div>
          </>
        }
      />
    </Routes>
  );
};
