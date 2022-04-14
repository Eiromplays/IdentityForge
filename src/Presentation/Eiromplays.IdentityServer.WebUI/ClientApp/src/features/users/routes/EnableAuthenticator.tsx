import QRCode from 'react-qr-code';

import { Spinner } from '@/components/Elements';
import { ContentLayout } from '@/components/Layout';
import { useAuth } from '@/lib/auth';

import { useEnableAuthenticator } from '../api/getEnableAuthenticator';
import { AddAuthenticator } from '../components/AddAuthenticator';

export const EnableAuthenticator = () => {
  const { user } = useAuth();
  const enableAuthenticatorQuery = useEnableAuthenticator();

  if (enableAuthenticatorQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!enableAuthenticatorQuery.data || !user) return null;

  return (
    <ContentLayout title="Configure Two-factor Authentication (2FA)">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              User security
            </h3>
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Scan this QRCode or use this key{' '}
            <code className="text-green-500">{enableAuthenticatorQuery.data.sharedKey}</code>
          </p>
        </div>
        <div className="border-t border-gray-200 flex gap-5 pt-5 pl-5 pb-5">
          <QRCode value={enableAuthenticatorQuery.data.authenticatorUri} />
          <AddAuthenticator />
        </div>
      </div>
    </ContentLayout>
  );
};
