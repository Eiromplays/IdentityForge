import { useParams } from 'react-router-dom';

import { Layout } from '../components/Layout';
import { Login2faForm } from '../components/Login2faForm';

export const Login2fa = () => {
  const { rememberMe, returnUrl } = useParams();

  const rememberMeAsBoolean = rememberMe?.toLowerCase() === 'true' ? true : false;

  return (
    <Layout title="Log in to your account with two-factor authentication">
      <Login2faForm
        rememberMe={rememberMeAsBoolean}
        returnUrl={returnUrl ?? ''}
        onSuccess={() => window.location.assign('/bff/login')}
      />
    </Layout>
  );
};
