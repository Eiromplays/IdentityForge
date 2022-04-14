import { useState } from 'react';

import { Button } from '@/components/Elements';
import { ContentLayout } from '@/components/Layout';
import { useAuth } from '@/lib/auth';

import { useRemoveAuthenticator } from '../api/removeAuthenticator';
import { AddAuthenticator } from '../components/AddAuthenticator';
import { ShowRecoveryCodes } from '../components/ShowRecoveryCodes';

export const TwoFactorAuthentication = () => {
  const [addAuthenticator, setAddAuthenticator] = useState(false);
  const [recoveryCodes, setRecoveryCodes] = useState([]);
  const { user } = useAuth();
  const removeAuthenticatorMutation = useRemoveAuthenticator();

  if (!user) return null;

  console.log(recoveryCodes, addAuthenticator);

  return (
    <ContentLayout title="Two-factor Authentication (2FA)">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              User security
            </h3>
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Secure your account with a extra layer of security.
          </p>
        </div>
        <div className="border-t border-gray-200 flex gap-5 pt-5 pl-5 pb-5">
          {recoveryCodes.length > 0 && <ShowRecoveryCodes codes={recoveryCodes} />}
          {!addAuthenticator && recoveryCodes?.length <= 0 && (
            <Button onClick={() => setAddAuthenticator(true)} variant="primary" size="sm">
              Add Authenticator
            </Button>
          )}
          <Button
            onClick={async () => await removeAuthenticatorMutation.mutateAsync(undefined)}
            isLoading={removeAuthenticatorMutation.isLoading}
            variant="danger"
            size="sm"
          >
            Remove Authenticator
          </Button>
          {addAuthenticator && recoveryCodes?.length <= 0 && (
            <AddAuthenticator
              onSuccess={(data) => {
                if (data?.length > 0) {
                  setRecoveryCodes(data);
                  setAddAuthenticator(false);
                }
              }}
            />
          )}
        </div>
      </div>
    </ContentLayout>
  );
};
