import { ContentLayout } from '@/components/Layout';
import { useAuth } from '@/lib/auth';

export const PersonalData = () => {
  const { user } = useAuth();

  if (!user) return null;

  return (
    <ContentLayout title="Personal Data">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              User Information
            </h3>
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Personal details of the user.
          </p>
        </div>
      </div>
    </ContentLayout>
  );
};
