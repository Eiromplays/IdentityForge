import { ContentLayout } from '@/components/Layout';
import { useAuth } from '@/lib/auth';

import { UpdateProfile } from '../components/UpdateProfile';

type EntryProps = {
  label: string;
  value: string;
};

const Entry = ({ label, value }: EntryProps) => (
  <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
    <dt className="text-sm font-medium text-gray-500 dark:text-white">{label}</dt>
    <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">{value}</dd>
  </div>
);

const PictureEntry = ({ label, value }: EntryProps) => (
  <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
    <dt className="text-sm font-medium text-gray-500 dark:text-white">{label}</dt>
    <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">
      <img width="200" height="200" className="rounded-full" src={value} alt="" />
    </dd>
  </div>
);

export const Profile = () => {
  const { user } = useAuth();

  if (!user) return null;

  return (
    <ContentLayout title="Profile">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              User Information
            </h3>
            <UpdateProfile />
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Personal details of the user.
          </p>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <dl className="sm:divide-y sm:divide-gray-200">
            <Entry label="Id" value={user.id} />
            <Entry label="Username" value={user.username} />
            <Entry label="Email Address" value={user.email} />
            {user.updated_at && (
              <Entry label="Last updated at" value={user.updated_at.toString()} />
            )}
            {user.created_at && <Entry label="Created at" value={user.created_at.toString()} />}
            {user.gravatarEmail && (
              <Entry label="Gravatar Email Address" value={user.gravatarEmail} />
            )}
            {user.profilePicture && (
              <PictureEntry label={'Profile Picture'} value={user.profilePicture} />
            )}
            {user.roles.length > 0 && (
              <Entry
                label={user.roles.length > 1 ? 'Roles' : 'Role'}
                value={user.roles.join(', ')}
              />
            )}
          </dl>
        </div>
      </div>
    </ContentLayout>
  );
};
