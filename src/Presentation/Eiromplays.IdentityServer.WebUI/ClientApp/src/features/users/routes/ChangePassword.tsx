import { ContentLayout } from 'eiromplays-ui';

import { ChangePasswordForm } from '../components/ChangePasswordForm';

export const ChangePassword = () => {
  return (
    <ContentLayout title="Change Password">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              Change password
            </h3>
          </div>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <ChangePasswordForm onSuccess={() => window.location.assign('/app')} />
        </div>
      </div>
    </ContentLayout>
  );
};
