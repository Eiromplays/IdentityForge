import { Table, Spinner } from '@/components/Elements';

import { useRoles } from '../api/getRoles';
import { Role } from '../types';

import { DeleteRole } from './DeleteRole';

export const RolesList = () => {
  const rolesQuery = useRoles();

  if (rolesQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!rolesQuery.data) return null;

  return (
    <Table<Role>
      data={rolesQuery.data}
      columns={[
        {
          title: 'Id',
          field: 'id',
        },
        {
          title: 'Name',
          field: 'name',
        },
        {
          title: 'Description',
          field: 'description',
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <DeleteRole id={id} />;
          },
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <a href={`/app/roles/${id}`}>View</a>;
          },
        },
      ]}
    />
  );
};
