import { ContentLayout, useAuth } from 'eiromplays-ui';

type ShowRecoveryCodesProps = {
  codes: any[];
};

export const ShowRecoveryCodes = ({ codes }: ShowRecoveryCodesProps) => {
  const { user } = useAuth();

  if (!codes || !user) return null;

  return (
    <ContentLayout title="Recovery Codes">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-red-800 dark:text-red-600">
              Make sure you store these codes in a safe place
            </h3>
          </div>
          <p className="mt-1 max-w-2xl text-sm text-red-700 dark:text-red-500">
            If you lose your device, or access to your recovery codes you will loose access to your
            account
          </p>
        </div>
        {codes.map((code) => {
          return (
            <p className="text-gray-800 dark:text-white pl-10 pb-4" key={code}>
              {code}
            </p>
          );
        })}
      </div>
    </ContentLayout>
  );
};
