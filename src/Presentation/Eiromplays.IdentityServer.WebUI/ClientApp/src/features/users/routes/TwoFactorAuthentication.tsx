import { useNavigate } from 'react-router-dom';

import { Button } from '@/components/Elements';
import { ContentLayout } from '@/components/Layout';
import { useAuth } from '@/lib/auth';

export const TwoFactorAuthentication = () => {
  const { user } = useAuth();
  const navigate = useNavigate();

  if (!user) return null;

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
          <Button onClick={() => navigate('./enable')} variant="primary" size="sm">
            Add Authenticator app
          </Button>
        </div>
      </div>
    </ContentLayout>
  );
};
