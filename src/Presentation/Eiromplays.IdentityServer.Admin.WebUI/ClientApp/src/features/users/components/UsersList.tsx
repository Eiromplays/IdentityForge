import { useSearch, MatchRoute } from '@tanstack/react-location';
import {
  defaultPageIndex,
  defaultPageSize,
  formatDate,
  Link,
  PaginatedTable,
  Spinner,
  useAuth,
  SearchFilter,
} from 'eiromplays-ui';
import React from 'react';

import { LocationGenerics } from '@/App';

import { SearchUserDTO } from '../api/searchUsers';
import { User } from '../types';

import { DeleteUser } from './DeleteUser';

export const UsersList = () => {
  const { user } = useAuth();
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  const searchFilter: SearchFilter = {
    customProperties: [{ name: 'isActive', value: true, type: 'checkbox' }],
    orderBy: ['userName'],
    advancedSearch: {
      fields: ['userName', 'email'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <>
      <PaginatedTable<SearchUserDTO, User>
        url="/users/search"
        queryKeyName="search-users"
        searchData={{ pageNumber: page, pageSize: pageSize }}
        searchFilter={searchFilter}
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
