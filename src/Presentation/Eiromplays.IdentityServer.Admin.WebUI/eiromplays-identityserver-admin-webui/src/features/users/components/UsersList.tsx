import { useSearchParams } from 'react-router-dom';

import { PaginatedTable, Spinner } from '@/components/Elements';
import { formatDate } from '@/utils/format';

import { SearchUserDTO, useSearchUsers } from '../api/searchUsers';
import { User } from '../types';

import { DeleteUser } from './DeleteUser';
import { UserRoles } from './UserRoles';

export const UsersList = () => {
  const [searchParams] = useSearchParams();
  const page = parseInt(searchParams.get('page') || '1', 10);

  const searchUserDto: SearchUserDTO = {
    pageNumber: page,
    pageSize: 10,
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
            return <a href={`/app/profile/${id}`}>Profile</a>;
          },
        },
      ]}
    />
  );
};
