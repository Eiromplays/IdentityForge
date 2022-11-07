import { Link } from 'eiromplays-ui';
import React from 'react';

import { Layout } from '../components/Layout';

export const ConfirmEmail = () => {
  return (
    <Layout title="Please check your email to verify your account.">
      <h1 className="text-center">Already verified?</h1>
      <div className="mt-2 gap-5 flex items-center justify-center">
        <div className="text-sm">
          <Link to="../forgot-password" className="font-medium text-blue-600 hover:text-blue-500">
            Forgot password?
          </Link>
        </div>
        <div className="text-sm">
          <Link to="../login" className="font-medium text-blue-600 hover:text-blue-500">
            Login
          </Link>
        </div>
      </div>
    </Layout>
  );
};
