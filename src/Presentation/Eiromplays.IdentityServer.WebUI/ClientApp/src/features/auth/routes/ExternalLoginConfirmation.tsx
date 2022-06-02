import { useMatch, useSearch } from '@tanstack/react-location';
import { Spinner } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useLogin } from '../api/getLogin';
import { ExternalLoginConfirmationForm } from '../components/ExternalLoginConfirmationForm';
import { Layout } from '../components/Layout';

export const ExternalLoginConfirmation = () => {
  const {
    params: { email, userName, loginProvider },
  } = useMatch<LocationGenerics>();

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
    <Layout title={`Associate your ${loginProvider} with account.`}>
      <ExternalLoginConfirmationForm email={email} userName={userName} returnUrl={returnUrl} />
    </Layout>
  );
};
