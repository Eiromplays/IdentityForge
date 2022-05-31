import { useSearch } from '@tanstack/react-location';
import { Spinner } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useLogin } from '../api/getLogin';
import { ExternalLoginProviders } from '../components/ExternalLoginProviders';
import { Layout } from '../components/Layout';
import { LoginForm } from '../components/LoginForm';

export const Login = () => {
  const { returnUrl } = useSearch<LocationGenerics>();

  const loginQuery = useLogin({ returnUrl });

  if (loginQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!loginQuery.data) return null;

  return (
    <Layout title="Log in to your account">
      <LoginForm onSuccess={() => window.location.assign('/bff/login')} />
      <ExternalLoginProviders
        title="External Login Providers:"
        externalProviders={loginQuery.data.visibleExternalProviders}
      />
    </Layout>
  );
};
