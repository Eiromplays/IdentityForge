import { Spinner } from '@/components/Elements';
import { ContentLayout } from '@/components/Layout';
import { useAuth } from '@/lib/auth';

import { useTwoFactorAuthentication } from '../api/getTwoFactorAuthentication';
import { AddAuthenticator } from '../components/AddAuthenticator';
import { DisableAuthenticator } from '../components/DisableAuthenticator';
import { ForgetTwoFactorClient } from '../components/ForgetTwoFactorClient';
import { GenerateRecoveryCodes } from '../components/GenerateRecoveryCodes';
import { ResetAuthenticator } from '../components/ResetAuthenticator';

export const TwoFactorAuthentication = () => {
  const { user } = useAuth();
  const twoFactorAuthenticationQuery = useTwoFactorAuthentication();

  if (twoFactorAuthenticationQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!twoFactorAuthenticationQuery.data || !user) return null;

  return (
    <ContentLayout title="Two-factor Authentication (2FA)">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              Two-Factor Authentication settings
            </h3>
          </div>
        </div>
        <div className="border-t border-gray-200 flex flex-wrap flex-column gap-5 pt-5 pl-5 pb-5">
          {twoFactorAuthenticationQuery.data.isMachineRemembered && <ForgetTwoFactorClient />}
          {twoFactorAuthenticationQuery.data.hasAuthenticator && (
            <>
              <DisableAuthenticator />
              <GenerateRecoveryCodes />
            </>
          )}
        </div>
      </div>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg mt-5">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              Authenticator app
            </h3>
          </div>
        </div>
        <div className="border-t border-gray-200 flex flex-wrap flex-column gap-5 pt-5 pl-5 pb-5">
          {twoFactorAuthenticationQuery.data.hasAuthenticator && <ResetAuthenticator />}
          <AddAuthenticator />
        </div>
      </div>
    </ContentLayout>
  );
};
