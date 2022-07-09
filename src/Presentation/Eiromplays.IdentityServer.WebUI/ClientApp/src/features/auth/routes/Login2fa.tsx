import { useSearch } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { Layout } from '../components/Layout';
import { Login2faForm } from '../components/Login2faForm';

export const Login2fa = () => {
  const { rememberMe, returnUrl } = useSearch<LocationGenerics>();

  const rememberMeAsBoolean = rememberMe?.toLowerCase() === 'true';

  return (
    <Layout title="Log in to your account with two-factor authentication">
      <Login2faForm rememberMe={rememberMeAsBoolean} returnUrl={returnUrl || ''} />
    </Layout>
  );
};
