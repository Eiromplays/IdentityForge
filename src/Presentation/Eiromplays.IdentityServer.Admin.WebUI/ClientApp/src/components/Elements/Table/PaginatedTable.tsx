import { HiOutlineArchive } from 'react-icons/hi';

import { Pagination, PaginationProps } from '../Pagination';

import { BaseEntry, Table, TableColumn } from './Table';

export type PaginatedTableProps<Entry> = PaginationProps<Entry> & {
  data: Entry[];
  columns: TableColumn<Entry>[];
};

export const PaginatedTable = <Entry extends BaseEntry | any>({
  paginationResponse,
  data,
  columns,
}: PaginatedTableProps<Entry>) => {
  if (!data?.length) {
    return (
      <div className="bg-white dark:bg-lighter-black text-gray-500 dark:text-white h-80 flex justify-center items-center flex-col">
        <HiOutlineArchive className="h-16 w-16" />
        <h4>No Entries Found</h4>
      </div>
    );
  }

  return (
    <div>
      <Table data={data} columns={columns} />
      <Pagination paginationResponse={paginationResponse} />
    </div>
  );
};
