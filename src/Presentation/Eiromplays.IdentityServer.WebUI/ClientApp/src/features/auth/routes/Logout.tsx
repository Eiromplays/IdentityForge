import { Layout } from '../components/Layout';
import { LogoutForm } from '../components/LogoutForm';

export const Logout = () => {
  return (
    <Layout title="Log out of your account">
      <LogoutForm onSuccess={() => window.location.assign('/')} />
    </Layout>
  );
};
