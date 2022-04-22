import { Table, Spinner, Link } from '@/components/Elements';
import { formatDate, formatLogType } from '@/utils/format';

import { useLogs } from '../api/getLogs';
import { Log } from '../types';

export const LogsList = () => {
  const logsQuery = useLogs();

  if (logsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!logsQuery.data) return null;

  return (
    <Table<Log>
      data={logsQuery.data}
      columns={[
        {
          title: 'Id',
          field: 'id',
        },
        {
          title: 'Type',
          field: 'type',
          Cell({ entry: { type } }) {
            return <span className={`${formatLogType(type)}`}>{type}</span>;
          },
        },
        {
          title: 'Affected columns',
          field: 'affectedColumns',
        },
        {
          title: 'Date',
          field: 'dateTime',
          Cell({ entry: { dateTime } }) {
            return <span>{formatDate(dateTime)}</span>;
          },
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { id } }) {
            return <Link to={`./${id}`}>View</Link>;
          },
        },
      ]}
    />
  );
};
