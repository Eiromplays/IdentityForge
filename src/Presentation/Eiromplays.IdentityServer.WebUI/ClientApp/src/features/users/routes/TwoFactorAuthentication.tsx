import { Spinner, ContentLayout, useAuth } from 'eiromplays-ui';

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
      {twoFactorAuthenticationQuery.data.is2FaEnabled && (
        <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
          <div className="px-4 py-5 sm:px-6">
            <div className="flex justify-between">
              <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
                Two-Factor Authentication settings
              </h3>
            </div>
          </div>
          {twoFactorAuthenticationQuery.data.recoveryCodesLeft == 0 ? (
            <div className="flex justify-center items-center">
              <p>You have no recovery codes left.</p>
              <GenerateRecoveryCodes />
            </div>
          ) : twoFactorAuthenticationQuery.data.recoveryCodesLeft == 1 ? (
            <div className="flex justify-center items-center">
              <p>You have one recovery code left.</p>
              <GenerateRecoveryCodes />
            </div>
          ) : twoFactorAuthenticationQuery.data.recoveryCodesLeft <= 3 ? (
            <div className="flex justify-center items-center">
              <p>
                You have {twoFactorAuthenticationQuery.data.recoveryCodesLeft} recovery codes left.
              </p>
              <GenerateRecoveryCodes />
            </div>
          ) : null}
          <div className="border-t border-gray-200 flex flex-wrap flex-column gap-5 pt-5 pl-5 pb-5">
            {twoFactorAuthenticationQuery.data.isMachineRemembered && <ForgetTwoFactorClient />}
            {twoFactorAuthenticationQuery.data.is2FaEnabled && (
              <>
                <DisableAuthenticator />
                <GenerateRecoveryCodes />
              </>
            )}
          </div>
        </div>
      )}
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
          <AddAuthenticator
            options={twoFactorAuthenticationQuery.data.validProviders.map((validProvider) => {
              return { label: validProvider, value: validProvider };
            })}
          />
        </div>
      </div>
    </ContentLayout>
  );
};
