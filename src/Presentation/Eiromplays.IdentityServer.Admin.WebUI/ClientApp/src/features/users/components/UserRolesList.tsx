import { Spinner, Table } from 'eiromplays-ui';

import { useUserRoles } from '../api/getUserRoles';
import { UserRole } from '../types';

type UserRolesListProps = {
  id: string;
};

export const UserRolesList = ({ id }: UserRolesListProps) => {
  const userRolesQuery = useUserRoles({ userId: id });

  if (userRolesQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!userRolesQuery.data) return null;

  return (
    <Table<UserRole>
      data={userRolesQuery.data}
      columns={[
        {
          title: 'Id',
          field: 'roleId',
        },
        {
          title: 'Name',
          field: 'roleName',
        },
        {
          title: 'Description',
          field: 'description',
        },
        {
          title: 'Enabled',
          field: 'enabled',
          Cell({ entry: { enabled } }) {
            return (
              <span className={enabled ? 'text-green-500' : 'text-red-500'}>
                {enabled.toString()}
              </span>
            );
          },
        },
      ]}
    />
  );
};
