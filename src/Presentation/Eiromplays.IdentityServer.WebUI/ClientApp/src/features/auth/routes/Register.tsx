import { useNavigate } from '@tanstack/react-location';

import { Layout } from '../components/Layout';
import { RegisterForm } from '../components/RegisterForm';

export const Register = () => {
  const navigate = useNavigate();

  return (
    <Layout title="Register your account">
      <RegisterForm onSuccess={() => navigate({ to: '/auth/login' })} />
    </Layout>
  );
};
