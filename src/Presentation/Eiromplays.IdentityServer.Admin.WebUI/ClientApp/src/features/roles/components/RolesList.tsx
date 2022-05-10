import { useSearch, MatchRoute } from '@tanstack/react-location';
import { Spinner, PaginatedTable, Link } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { SearchRoleDTO, useSearchRoles } from '../api/searchRoles';
import { Role } from '../types';

import { DeleteRole } from './DeleteRole';

export const RolesList = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || 1;
  const pageSize = search.pagination?.size || 10;

  const searchRoleDto: SearchRoleDTO = {
    pageNumber: page,
    pageSize: pageSize,
  };

  const rolesQuery = useSearchRoles({ data: searchRoleDto });

  if (rolesQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!rolesQuery.data) return null;

  return (
    <PaginatedTable<Role>
      paginationResponse={rolesQuery.data}
      data={rolesQuery.data?.data}
      onPageSizeChanged={rolesQuery.remove}
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
