import { useSearch } from '@tanstack/react-location';
import { defaultPageIndex, defaultPageSize, PaginatedTable, SearchFilter } from 'eiromplays-ui';
import React from 'react';

import { LocationGenerics } from '@/App';
import { UserProvider } from '@/features/users';

import { SearchUserProvidersDTO } from '../api/searchUserProviders';
import { DeleteUserProvider } from '../components/DeleteUserProvider';

export type UserProvidersListProps = {
  id: string;
};

export const UserProvidersList = ({ id }: UserProvidersListProps) => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: ['loginProvider', 'providerKey', 'providerDisplayName', 'loginProvider Desc'],
    advancedSearch: {
      fields: ['loginProvider', 'providerKey', 'providerDisplayName'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <>
      <PaginatedTable<SearchUserProvidersDTO, UserProvider>
        url={`/users/${id}/providers-search`}
        queryKeyName={`search-user-providers-${id}`}
        searchData={{ pageNumber: page, pageSize: pageSize }}
        searchFilter={searchFilter}
        columns={[
          {
            title: 'LoginProvider',
            field: 'loginProvider',
          },
          {
            title: 'ProviderKey',
            field: 'providerKey',
          },
          {
            title: 'ProviderDisplayName',
            field: 'providerDisplayName',
          },
          {
            title: '',
            field: 'providerKey',
            Cell({ entry: { loginProvider, providerKey } }) {
              return (
                <DeleteUserProvider
                  id={id}
                  loginProvider={loginProvider}
                  providerKey={providerKey}
                />
              );
            },
          },
        ]}
      />
    </>
  );
};
