import { Table, Spinner, useAuth } from 'eiromplays-ui';

import { useUserLogins } from '../api/getLogins';
import { AddUserLogin } from '../components/AddUserLogin';
import { AuthenticationScheme } from '../types';

export const OtherLoginsList = () => {
  const { user } = useAuth();
  const userLoginsQuery = useUserLogins();

  if (userLoginsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!userLoginsQuery.data || !user) return null;

  return (
    <Table<AuthenticationScheme>
      data={userLoginsQuery.data.otherLogins}
      columns={[
        {
          title: 'Name',
          field: 'name',
        },
        {
          title: 'Display Name',
          field: 'displayName',
        },
        {
          title: '',
          field: 'displayName',
          Cell({ entry: { name } }) {
            return <AddUserLogin providerName={name} />;
          },
        },
      ]}
    />
  );
};
