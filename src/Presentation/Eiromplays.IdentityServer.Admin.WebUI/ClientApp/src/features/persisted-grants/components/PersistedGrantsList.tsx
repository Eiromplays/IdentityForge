import { Table, Spinner, Link } from '@/components/Elements';
import { formatDate } from '@/utils/format';

import { usePersistedGrants } from '../api/getPersistedGrants';
import { PersistedGrant } from '../types';

import { DeletePersistedGrant } from './DeletePersistedGrant';

export const PersistedGrantsList = () => {
  const grantsQuery = usePersistedGrants();

  if (grantsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!grantsQuery.data) return null;

  return (
    <Table<PersistedGrant>
      data={grantsQuery.data}
      columns={[
        {
          title: 'Type',
          field: 'type',
        },
        {
          title: 'Client Id',
          field: 'clientId',
        },
        {
          title: 'Subject Id',
          field: 'subjectId',
        },
        {
          title: 'Description',
          field: 'description',
        },
        {
          title: 'Created At',
          field: 'creationTime',
          Cell({ entry: { creationTime } }) {
            return <span>{formatDate(creationTime)}</span>;
          },
        },
        {
          title: 'Expires At',
          field: 'expiration',
          Cell({ entry: { expiration } }) {
            return <span>{formatDate(expiration)}</span>;
          },
        },
        {
          title: '',
          field: 'key',
          Cell({ entry: { key } }) {
            return <Link to={`./${key}`}>View</Link>;
          },
        },
        {
          title: '',
          field: 'key',
          Cell({ entry: { key } }) {
            return <DeletePersistedGrant key={key} />;
          },
        },
      ]}
    />
  );
};
