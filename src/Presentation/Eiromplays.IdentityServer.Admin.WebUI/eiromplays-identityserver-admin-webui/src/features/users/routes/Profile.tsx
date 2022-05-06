
import { Spinner, ContentLayout } from 'eiromplays-ui';

import { useUser } from '../api/getUser';
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
  //const { id } = useParams();

  const userQuery = useUser({ userId: '' });

  if (userQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!userQuery.data) return null;

  return (
    <ContentLayout title="Profile">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              User Information
            </h3>
            <UpdateProfile id={''} />
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Personal details of the user.
          </p>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <dl className="sm:divide-y sm:divide-gray-200">
            <Entry label="Id" value={userQuery.data.id} />
            <Entry label="Username" value={userQuery.data.userName} />
            <Entry label="First Name" value={userQuery.data.firstName} />
            <Entry label="Last Name" value={userQuery.data.lastName} />
            <Entry label="Email Address" value={userQuery.data.email} />
            {userQuery.data.updated_at && (
              <Entry label="Last updated at" value={userQuery.data.updated_at.toString()} />
            )}
            {userQuery.data.created_at && (
              <Entry label="Created at" value={userQuery.data.created_at.toString()} />
            )}
            {userQuery.data.gravatarEmail && (
              <Entry label="Gravatar Email Address" value={userQuery.data.gravatarEmail} />
            )}
            {userQuery.data.profilePicture && (
              <PictureEntry label={'Profile Picture'} value={userQuery.data.profilePicture} />
            )}
            {userQuery.data.roles?.length > 0 && (
              <Entry
                label={userQuery.data.roles.length > 1 ? 'Roles' : 'Role'}
                value={userQuery.data.roles.join(', ')}
              />
            )}
          </dl>
        </div>
      </div>
    </ContentLayout>
  );
};
