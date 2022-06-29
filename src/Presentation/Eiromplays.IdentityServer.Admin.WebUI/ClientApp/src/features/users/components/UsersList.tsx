import { useSearch, MatchRoute } from '@tanstack/react-location';
import {
  Button,
  defaultPageIndex,
  defaultPageSize,
  formatDate,
  Link,
  PaginatedTable,
  Spinner,
  useAuth,
} from 'eiromplays-ui';
import React from 'react';

import { LocationGenerics } from '@/App';
import { SearchFilter } from '@/features/users/components/SearchFilter';

import { SearchUserDTO } from '../api/searchUsers';
import { User } from '../types';

import { DeleteUser } from './DeleteUser';

export const UsersList = () => {
  const { user } = useAuth();
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  console.log(search.searchFilter);

  const searchData: SearchUserDTO = { pageNumber: page, pageSize: pageSize };
  const filter: SearchFilter = {
    customFilters: [{ name: 'isActive', value: false, formType: 'checkbox' }],
    orderBy: ['userName'],
    advancedSearch: {
      fields: ['userName', 'email'],
      keyword: '',
    },
    keyword: '',
  };
  return (
    <>
      <SearchFilter filter={filter} />
      <PaginatedTable<SearchUserDTO, User>
        url="/users/search"
        queryKeyName="search-users"
        searchData={searchData}
        columns={[
          {
            title: 'Username',
            field: 'userName',
          },
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
            title: 'Created',
            field: 'createdOn',
            Cell({ entry: { createdOn } }) {
              return <span>{formatDate(createdOn)}</span>;
            },
          },
          {
            title: 'Last Modified',
            field: 'lastModifiedOn',
            Cell({ entry: { lastModifiedOn } }) {
              return <span>{formatDate(lastModifiedOn)}</span>;
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
              return user.id !== id ? (
                <Link to={id} search={search} className="block">
                  <pre className={`text-sm`}>
                    View{' '}
                    <MatchRoute to={id} pending>
                      <Spinner size="md" className="inline-block" />
                    </MatchRoute>
                  </pre>
                </Link>
              ) : (
                <></>
              );
            },
          },
        ]}
      />
    </>
  );
};
