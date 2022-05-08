import { useSearch, MatchRoute } from '@tanstack/react-location';
import { formatDate, Link, PaginatedTable, Spinner } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchUserDTO, useSearchUsers } from '../api/searchUsers';
import { User } from '../types';

import { DeleteUser } from './DeleteUser';

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

  return (
    <PaginatedTable<User>
      paginationResponse={searchUsersQuery.data}
      data={searchUsersQuery.data.data}
      onPageSizeChanged={searchUsersQuery.remove}
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
            return (
              <Link to={`${id}/roles`} search={search} className="block">
                <pre className={`text-sm`}>
                  Roles{' '}
                  <MatchRoute to={`${id}/roles`} pending>
                    <Spinner size="md" className="inline-block" />
                  </MatchRoute>
                </pre>
              </Link>
            );
          },
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return (
              <Link to={id} search={search} className="block">
                <pre className={`text-sm`}>
                  View{' '}
                  <MatchRoute to={id} pending>
                    <Spinner size="md" className="inline-block" />
                  </MatchRoute>
                </pre>
              </Link>
            );
          },
        },
      ]}
    />
  );
};
