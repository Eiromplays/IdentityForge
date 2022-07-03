import { useSearch } from '@tanstack/react-location';
import { Spinner } from 'eiromplays-ui';
import React from 'react';
import Select from 'react-select';

import { LocationGenerics } from '@/App';

import { useLogin } from '../api/getLogin';
import { ExternalLoginProviders } from '../components/ExternalLoginProviders';
import { Layout } from '../components/Layout';
import { LoginForm } from '../components/LoginForm';
import { LoginWithPhoneNumberForm } from '../components/LoginWithPhoneNumberForm';

const options = [
  { value: 'email', label: 'Email' },
  { value: 'phone', label: 'Phone' },
];

export const Login = () => {
  const { returnUrl, ReturnUrl } = useSearch<LocationGenerics>();

  const loginQuery = useLogin({ returnUrl: returnUrl || ReturnUrl });

  const [provider, setProvider] = React.useState<string>('email');

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
      <Select
        options={options}
        onChange={(value) => setProvider(value?.value || '')}
        value={{ value: provider, label: provider }}
      />
      {provider === 'email' && <LoginForm />}
      {provider === 'phone' && <LoginWithPhoneNumberForm />}
      <ExternalLoginProviders
        title="External Login Providers:"
        externalProviders={loginQuery.data.visibleExternalProviders}
      />
    </Layout>
  );
};
