import { Table, Spinner, useAuth } from 'eiromplays-ui';

import { useUserLogins } from '../api/getLogins';
import { UserLogin } from '../types';

import { RemoveUserLogin } from './RemoveUserLogin';

export const UserLoginsList = () => {
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
    <Table<UserLogin>
      data={userLoginsQuery.data.currentLogins}
      columns={[
        {
          title: 'Login Provider',
          field: 'loginProvider',
        },
        {
          title: 'Provider Key',
          field: 'providerKey',
        },
        {
          title: 'Provider Display Name',
          field: 'providerDisplayName',
        },
        {
          title: '',
          field: 'loginProvider',
          Cell({ entry: { loginProvider, providerKey } }) {
            return userLoginsQuery.data.showRemoveButton ? (
              <RemoveUserLogin loginProvider={loginProvider} providerKey={providerKey} />
            ) : (
              <></>
            );
          },
        },
      ]}
    />
  );
};
