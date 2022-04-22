import { useParams } from 'react-router-dom';

import { Spinner } from '@/components/Elements';

import { useLogin } from '../api/getLogin';
import { ExternalLoginConfirmationForm } from '../components/ExternalLoginConfirmationForm';
import { Layout } from '../components/Layout';

export const ExternalLoginConfirmation = () => {
  const { email, userName, loginProvider } = useParams();

  //TODO: Find a better way to get the returnUrl
  let returnUrl = '';
  const idx = location.href.toLowerCase().indexOf('?returnurl=');
  if (idx > 0) {
    returnUrl = location.href.substring(idx + 11);
  }

  console.log('ExternalLoginConfirmation', { email, userName, returnUrl, loginProvider });

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
