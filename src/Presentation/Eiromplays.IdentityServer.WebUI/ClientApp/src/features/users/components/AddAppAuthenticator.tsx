import { Spinner } from 'eiromplays-ui';

import { useEnableAuthenticator } from '../api/getEnableAuthenticator';

export const AddAppAuthenticator = () => {
  const enableAuthenticatorQuery = useEnableAuthenticator();

  if (enableAuthenticatorQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!enableAuthenticatorQuery.data) return null;

  return (
    <>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Scan this QRCode or use this key{' '}
            <code className="text-green-500">{enableAuthenticatorQuery.data.sharedKey}</code>
          </p>
        </div>
      </div>
    </>
  );
};
