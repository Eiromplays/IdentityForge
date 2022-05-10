import { Button, ConfirmationDialog, ContentLayout, useAuth } from 'eiromplays-ui';

import { useDeleteUser } from '../api/deleteUser';
import { useExportPersonalData } from '../api/exportPersonalData';

export const PersonalData = () => {
  const { user } = useAuth();
  const exportPersonalDataMutation = useExportPersonalData();
  const deleteUserMutation = useDeleteUser();

  if (!user) return null;

  return (
    <ContentLayout title="Personal Data">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              User management
            </h3>
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Personal data and information.
          </p>
        </div>
        <div className="border-t border-gray-200 flex gap-5 pt-5 pl-5 pb-5">
          <ConfirmationDialog
            icon="danger"
            title="Delete Account"
            body="Are you sure you want to delete your account? There is no way to undo this action!"
            triggerButton={
              <Button size="sm" variant="danger" isLoading={deleteUserMutation.isLoading}>
                Delete Account
              </Button>
            }
            confirmButton={
              <Button
                variant="danger"
                size="sm"
                isLoading={deleteUserMutation.isLoading}
                onClick={async () => {
                  await deleteUserMutation.mutateAsync(undefined);
                  if (user?.logoutUrl) window.location.assign(user.logoutUrl);
                }}
              >
                Proceed
              </Button>
            }
          />
          <Button
            variant="primary"
            size="sm"
            isLoading={exportPersonalDataMutation.isLoading}
            onClick={async () => await exportPersonalDataMutation.mutateAsync(undefined)}
          >
            Download Personal Data
          </Button>
        </div>
      </div>
    </ContentLayout>
  );
};
