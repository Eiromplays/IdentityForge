import { useSearch, MatchRoute } from '@tanstack/react-location';
import {
  Spinner,
  PaginatedTable,
  Link,
  defaultPageIndex,
  defaultPageSize,
  SearchFilter,
} from 'eiromplays-ui';
import React from 'react';

import { LocationGenerics } from '@/App';

import { SearchRoleDTO } from '../api/searchRoles';
import { Role } from '../types';

import { DeleteRole } from './DeleteRole';

export const RolesList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: ['id', 'name', 'description', 'permissions'],
    advancedSearch: {
      fields: ['id', 'name', 'description', 'permissions'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <PaginatedTable<SearchRoleDTO, Role>
      url="/roles/search"
      queryKeyName={['search-roles']}
      searchData={{ pageNumber: page, pageSize: pageSize }}
      searchFilter={searchFilter}
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
            return (
              <Link to={`${id}/claims`} search={search} className="block">
                <pre className={`text-sm`}>
                  Claims{' '}
                  <MatchRoute to={`${id}/claims`} pending>
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
