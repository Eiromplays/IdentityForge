import { useSearch } from '@tanstack/react-location';
import {
  defaultPageIndex,
  defaultPageSize,
  formatDate,
  PaginatedTable,
  SearchFilter,
} from 'eiromplays-ui';
import React from 'react';

import { LocationGenerics } from '@/App';

import { SearchUserClaimsDTO } from '../api/searchUserClaims';
import { UpdateUserClaim } from '../components/UpdateUserClaim';
import { UserClaim } from '../types';

import { DeleteUserClaim } from './DeleteUserClaim';

export type UserClaimsListProps = {
  id: string;
};

export const UserClaimsList = ({ id }: UserClaimsListProps) => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;

  const searchFilter: SearchFilter = {
    customProperties: [],
    orderBy: ['claimType', 'claimValue', 'createdOn', 'lastModifiedOn Desc'],
    advancedSearch: {
      fields: ['claimType', 'claimValue'],
      keyword: '',
    },
    keyword: '',
  };

  return (
    <>
      <PaginatedTable<SearchUserClaimsDTO, UserClaim>
        url={`/users/${id}/claims-search`}
        queryKeyName={[`search-user-claims-${id}`]}
        searchData={{ pageNumber: page, pageSize: pageSize }}
        searchFilter={searchFilter}
        columns={[
          {
            title: 'Type',
            field: 'type',
          },
          {
            title: 'Value',
            field: 'value',
          },
          {
            title: 'Value Type',
            field: 'valueType',
          },
          {
            title: 'Issuer',
            field: 'issuer',
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
            field: 'type',
            Cell({ entry: { type, value, valueType, issuer, createdOn, lastModifiedOn } }) {
              return (
                <UpdateUserClaim
                  id={id}
                  userClaim={{
                    type: type,
                    value: value,
                    valueType: valueType,
                    issuer: issuer,
                    createdOn: createdOn,
                    lastModifiedOn: lastModifiedOn,
                  }}
                />
              );
            },
          },
          {
            title: '',
            field: 'type',
            Cell({ entry: { type, value, valueType, issuer, createdOn, lastModifiedOn } }) {
              return (
                <DeleteUserClaim
                  id={id}
                  userClaim={{
                    type: type,
                    value: value,
                    valueType: valueType,
                    issuer: issuer,
                    createdOn: createdOn,
                    lastModifiedOn: lastModifiedOn,
                  }}
                />
              );
            },
          },
        ]}
      />
    </>
  );
};
