import React from 'react';

import { Layout } from '../components/Layout';
import { ResendEmailConfirmationForm } from '../components/ResendEmailConfirmationForm';

export const ResendEmailConfirmation = () => {
  return (
    <Layout title="Resend Email Confirmation">
      <ResendEmailConfirmationForm />
    </Layout>
  );
};
