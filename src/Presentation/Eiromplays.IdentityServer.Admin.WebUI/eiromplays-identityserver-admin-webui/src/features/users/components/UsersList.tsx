import { useSearch } from '@tanstack/react-location';
import { formatDate, Link, PaginatedTable, Spinner } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchUserDTO, useSearchUsers } from '../api/searchUsers';
import { User } from '../types';

import { DeleteUser } from './DeleteUser';
import { UserRoles } from './UserRoles';

export const UsersList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || 1;
  const pageSize = search.pagination?.size || 10;

  const searchUserDto: SearchUserDTO = {
    pageNumber: page,
    pageSize: pageSize,
    isActive: true,
  };

  const searchUsersQuery = useSearchUsers({ data: searchUserDto });

  if (searchUsersQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!searchUsersQuery.data?.data) return null;

  if (searchUsersQuery.data?.pageSize !== pageSize) {
    searchUsersQuery.refetch();
  }

  return (
    <PaginatedTable<User>
      paginationResponse={searchUsersQuery.data}
      data={searchUsersQuery.data.data}
      columns={[
        {
          title: 'First Name',
          field: 'firstName',
        },
        {
          title: 'Last Name',
          field: 'lastName',
        },
        {
          title: 'Email',
          field: 'email',
        },
        {
          title: 'Created At',
          field: 'created_at',
          Cell({ entry: { created_at } }) {
            return <span>{formatDate(created_at)}</span>;
          },
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <DeleteUser id={id} />;
          },
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <UserRoles id={id} />;
          },
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <Link to={`./${id}`}>View</Link>;
          },
        },
      ]}
    />
  );
};
