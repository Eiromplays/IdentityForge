import React from 'react';

import { Layout } from '../components/Layout';
import { ResendPhoneNumberConfirmationForm } from '../components/ResendPhoneNumberConfirmationForm';

export const ResendPhoneNumberConfirmation = () => {
  return (
    <Layout title="Resend PhoneNumber Confirmation">
      <ResendPhoneNumberConfirmationForm />
    </Layout>
  );
};
