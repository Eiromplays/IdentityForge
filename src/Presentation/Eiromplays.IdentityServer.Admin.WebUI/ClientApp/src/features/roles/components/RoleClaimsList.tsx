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

import { SearchRoleClaimsDTO } from '../api/searchRoleClaims';
import { RoleClaim } from '../types';

import { DeleteRoleClaim } from './DeleteRoleClaim';
import { UpdateRoleClaim } from './UpdateRoleClaim';

export type RoleClaimsListProps = {
  roleId: string;
};

export const RoleClaimsList = ({ roleId }: RoleClaimsListProps) => {
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
      <PaginatedTable<SearchRoleClaimsDTO, RoleClaim>
        url={`/roles/${roleId}/claims-search`}
        queryKeyName={[`search-role-claims`, roleId]}
        searchData={{ pageNumber: page, pageSize: pageSize }}
        searchFilter={searchFilter}
        columns={[
          {
            title: 'Id',
            field: 'id',
          },
          {
            title: 'Type',
            field: 'claim',
            Cell({ entry: { claim } }) {
              return <span>{claim.type}</span>;
            },
          },
          {
            title: 'Value',
            field: 'claim',
            Cell({ entry: { claim } }) {
              return <span>{claim.value}</span>;
            },
          },
          {
            title: 'Value Type',
            field: 'claim',
            Cell({ entry: { claim } }) {
              return <span>{claim.valueType}</span>;
            },
          },
          {
            title: 'Issuer',
            field: 'claim',
            Cell({ entry: { claim } }) {
              return <span>{claim.issuer}</span>;
            },
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
            Cell({ entry: { claim, id } }) {
              return (
                <UpdateRoleClaim
                  roleId={roleId}
                  roleClaim={{
                    createdOn: 0,
                    lastModifiedOn: 0,
                    id: id,
                    claim: claim,
                  }}
                />
              );
            },
          },
          {
            title: '',
            field: 'id',
            Cell({ entry: { id } }) {
              return <DeleteRoleClaim roleId={roleId} claimId={id} />;
            },
          },
        ]}
      />
    </>
  );
};
