// Original source code: https://github.com/estevanmaito/windmill-react-ui/blob/master/src/Pagination.tsx

import { useEffect, useState } from 'react';
import { HiOutlineChevronLeft, HiOutlineChevronRight } from 'react-icons/hi';
import { useSearchParams } from 'react-router-dom';

import { PaginationResponse } from '@/types';

import { Button } from '../Button';

export type PaginationProps<Entry> = {
  paginationResponse: PaginationResponse<Entry>;
};

export const Pagination = <Entry extends any>({ paginationResponse }: PaginationProps<Entry>) => {
  const [pages, setPages] = useState<(number | string)[]>([]);
  const [searchParams, setSearchParams] = useSearchParams();
  const page = parseInt(searchParams.get('page') || '1', 10);

  useEffect(() => {
    // [1], 2, 3, 4, 5, ..., 12 case #1
    // 1, [2], 3, 4, 5, ..., 12
    // 1, 2, [3], 4, 5, ..., 12
    // 1, 2, 3, [4], 5, ..., 12
    // 1, ..., 4, [5], 6, ..., 12 case #2
    // 1, ..., 5, [6], 7, ..., 12
    // 1, ..., 6, [7], 8, ..., 12
    // 1, ..., 7, [8], 9, ..., 12
    // 1, ..., 8, [9], 10, 11, 12 case #3
    // 1, ..., 8, 9, [10], 11, 12
    // 1, ..., 8, 9, 10, [11], 12
    // 1, ..., 8, 9, 10, 11, [12]
    // [1], 2, 3, 4, 5, ..., 8
    // always show first and last
    // max of 7 pages shown (incl. [...])
    if (paginationResponse.totalPages <= paginationResponse.pageSize) {
      setPages(Array.from({ length: paginationResponse.totalPages }).map((_, i) => i + 1));
    } else if (page < 5) {
      // #1 active page < 5 -> show first 5
      setPages([1, 2, 3, 4, 5, '...', paginationResponse.totalPages]);
    } else if (page >= 5 && page < paginationResponse.totalPages - 3) {
      // #2 active page >= 5 && < TOTAL_PAGES - 3
      setPages([1, '...', page - 1, page, page + 1, '...', paginationResponse.totalPages]);
    } else {
      // #3 active page >= TOTAL_PAGES - 3 -> show last
      setPages([
        1,
        '...',
        paginationResponse.totalPages - 4,
        paginationResponse.totalPages - 3,
        paginationResponse.totalPages - 2,
        paginationResponse.totalPages - 1,
        paginationResponse.totalPages,
      ]);
    }
  }, [page, paginationResponse]);

  const NextPage = () => {
    if (paginationResponse.hasNextPage) {
      searchParams.set('page', (page + 1).toString());
      setSearchParams(searchParams);
    }
  };

  const PreviousPage = () => {
    if (paginationResponse.hasPreviousPage) {
      searchParams.set('page', (page - 1).toString());
      setSearchParams(searchParams);
    }
  };

  const SetPage = (page: number) => {
    searchParams.set('page', page.toString());
    setSearchParams(searchParams);
  };

  return (
    <div>
      <div className="bg-white px-4 py-3 flex items-center justify-between border-t border-gray-200 sm:px-6">
        <div className="flex-1 flex justify-between sm:hidden">
          {paginationResponse.hasPreviousPage && (
            <a
              href="./"
              className="relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
              onClick={PreviousPage}
            >
              Previous
            </a>
          )}
          {paginationResponse.hasNextPage && (
            <a
              href="./"
              className="ml-3 relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
              onClick={NextPage}
            >
              Next
            </a>
          )}
        </div>
        <div className="hidden sm:flex-1 sm:flex sm:items-center sm:justify-between">
          <div>
            <p className="text-sm text-gray-700">
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
                      className="relative inline-flex items-center px-2 py-2 rounded-l-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50"
                      onClick={PreviousPage}
                    >
                      <span className="sr-only">Previous</span>
                      <HiOutlineChevronLeft className="h-5 w-5" aria-hidden="true" />
                    </Button>
                  )}
                </li>
                {pages.map((p, i) => (
                  <li key={p.toString() + i}>
                    {p === '...' ? (
                      <EmptyPageButton />
                    ) : (
                      <PageButton page={p} isActive={p === page} onClick={() => SetPage(+p)} />
                    )}
                  </li>
                ))}
                <li>
                  {paginationResponse.hasNextPage && (
                    <Button
                      className="relative inline-flex items-center px-2 py-2 rounded-r-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50"
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
      </div>
    </div>
  );
};

export const EmptyPageButton = () => <span className="px-2 py-1">...</span>;

interface PageButtonProps {
  /**
   * The page the button represents
   */
  page: string | number;
  /**
   * Defines if the button is active
   */
  isActive?: boolean;

  onClick: () => void;
}

export const PageButton: React.FC<PageButtonProps> = function PageButton({
  page,
  isActive,
  onClick,
}) {
  return (
    <Button size="sm" variant={isActive ? 'primary' : 'outline'} onClick={onClick}>
      {page}
    </Button>
  );
};
