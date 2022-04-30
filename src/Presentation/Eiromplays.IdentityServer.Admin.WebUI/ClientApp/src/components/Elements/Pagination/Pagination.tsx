// Original source code: https://github.com/estevanmaito/windmill-react-ui/blob/master/src/Pagination.tsx AND https://javascript.plainenglish.io/building-a-pagination-component-in-react-with-typescript-2e7f7b62b35d

import { HiOutlineChevronLeft, HiOutlineChevronRight } from 'react-icons/hi';
import { useSearchParams } from 'react-router-dom';
import { OnChangeValue } from 'react-select';
import CreatableSelect from 'react-select/creatable';

import { PaginationResponse } from '@/types';

import { Button } from '../Button';

import {
  defaultPaginationPageSizeOption,
  PaginationPageSizeOption,
  paginationPageSizeOptions,
} from './data';
import { EmptyPageButton } from './EmptyPageButton';
import { PageButton } from './PageButton';

export type PaginationProps<Entry> = {
  paginationResponse: PaginationResponse<Entry>;
};

export const Pagination = <Entry extends any>({ paginationResponse }: PaginationProps<Entry>) => {
  const [searchParams, setSearchParams] = useSearchParams();
  const { totalPages, currentPage: page } = paginationResponse;

  const NextPage = () => {
    if (!paginationResponse.hasNextPage) return;
    SetPage(page + 1);
  };

  const PreviousPage = () => {
    if (!paginationResponse.hasPreviousPage) return;
    SetPage(page - 1);
  };

  const SetPage = (page: number) => {
    searchParams.set('page', page.toString());
    setSearchParams(searchParams);
  };

  const SetPageSize = (pageSize: number) => {
    searchParams.set('pageSize', pageSize.toString());
    setSearchParams(searchParams);
  };

  const handleChange = (newValue: OnChangeValue<PaginationPageSizeOption, false>) => {
    SetPageSize(newValue?.value || 10);
  };
  const handleInputChange = (inputValue: any) => {
    SetPageSize(inputValue || 10);
  };

  return (
    <div>
      <div className="bg-white dark:bg-gray-800 px-4 py-3 flex items-center justify-between border-t border-gray-200 dark:border-gray-500 sm:px-6">
        <div className="flex-1 flex justify-between sm:hidden">
          {paginationResponse.hasPreviousPage && (
            <Button
              className="relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 dark:border-gray-600 dark:text-gray-200 bg-white hover:bg-gray-50"
              onClick={PreviousPage}
            >
              Previous
            </Button>
          )}
          {paginationResponse.hasNextPage && (
            <Button
              className="ml-3 relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 dark:border-gray-600 dark:text-gray-200 bg-white hover:bg-gray-50"
              onClick={NextPage}
            >
              Next
            </Button>
          )}
        </div>
        <div className="hidden sm:flex-1 sm:flex sm:items-center sm:justify-between">
          <div>
            <p className="text-sm text-gray-700 dark:text-gray-200">
              Showing{' '}
              <span className="font-medium">
                {paginationResponse.currentPage * paginationResponse.pageSize -
                  paginationResponse.pageSize +
                  1}
              </span>{' '}
              to{' '}
              <span className="font-medium">
                {Math.min(
                  paginationResponse.currentPage * paginationResponse.pageSize,
                  paginationResponse.totalCount
                )}
              </span>{' '}
              of <span className="font-medium">{paginationResponse.totalCount}</span> results
            </p>
          </div>
          <div>
            <nav
              className="relative z-0 inline-flex rounded-md shadow-sm -space-x-px"
              aria-label="Pagination"
            >
              <ul className="inline-flex items-center">
                <li>
                  {paginationResponse.hasPreviousPage && (
                    <Button
                      className="relative inline-flex items-center px-2 py-2 rounded-l-md border border-gray-300 dark:border-gray-600 dark:text-gray-50 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50"
                      onClick={PreviousPage}
                    >
                      <span className="sr-only">Previous</span>
                      <HiOutlineChevronLeft className="h-5 w-5" aria-hidden="true" />
                    </Button>
                  )}
                </li>
                <PageButton page={1} isActive={1 === page} onClick={SetPage} />
                {page > 3 && <EmptyPageButton />}
                {page === totalPages && totalPages > 3 && (
                  <PageButton page={page - 2} isActive={page - 2 === page} onClick={SetPage} />
                )}
                {page > 2 && (
                  <PageButton page={page - 1} isActive={page - 1 === page} onClick={SetPage} />
                )}
                {page !== 1 && page !== totalPages && (
                  <PageButton page={page} isActive={page === page} onClick={SetPage} />
                )}
                {page < totalPages - 1 && (
                  <PageButton page={page + 1} isActive={page + 1 === page} onClick={SetPage} />
                )}
                {page === 1 && totalPages > 3 && (
                  <PageButton page={page + 2} isActive={page + 2 === page} onClick={SetPage} />
                )}
                {page < totalPages - 2 && <EmptyPageButton />}
                {totalPages > 1 && (
                  <PageButton page={totalPages} isActive={totalPages === page} onClick={SetPage} />
                )}
                <li>
                  {paginationResponse.hasNextPage && (
                    <Button
                      className="relative inline-flex items-center px-2 py-2 rounded-r-md border border-gray-300 dark:border-gray-600 dark:text-gray-50 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50"
                      onClick={NextPage}
                    >
                      <span className="sr-only">Next</span>
                      <HiOutlineChevronRight className="h-5 w-5" aria-hidden="true" />
                    </Button>
                  )}
                </li>
              </ul>
            </nav>
          </div>
        </div>

        <CreatableSelect
          isClearable
          onChange={handleChange}
          onInputChange={handleInputChange}
          defaultValue={defaultPaginationPageSizeOption}
          options={paginationPageSizeOptions}
          className="ml-4"
        />
      </div>
    </div>
  );
};
