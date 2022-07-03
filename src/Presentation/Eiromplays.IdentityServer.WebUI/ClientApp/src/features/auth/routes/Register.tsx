import { useNavigate } from '@tanstack/react-location';
import React from 'react';
import Select from 'react-select';

import { Layout } from '../components/Layout';
import { RegisterForm } from '../components/RegisterForm';
import { RegisterUsingPhoneNumberForm } from '../components/RegisterUsingPhoneNumberForm';

const options = [
  { value: 'email', label: 'Email' },
  { value: 'phone', label: 'Phone' },
];

export const Register = () => {
  const navigate = useNavigate();
  const [provider, setProvider] = React.useState<string>('email');

  return (
    <Layout title="Register your account">
      <Select
        options={options}
        onChange={(value) => setProvider(value?.value || '')}
        value={{ value: provider, label: provider }}
      />
      {provider === 'phone' && (
        <RegisterUsingPhoneNumberForm onSuccess={() => navigate({ to: '/auth/login' })} />
      )}
      {provider === 'email' && <RegisterForm onSuccess={() => navigate({ to: '/auth/login' })} />}
    </Layout>
  );
};
