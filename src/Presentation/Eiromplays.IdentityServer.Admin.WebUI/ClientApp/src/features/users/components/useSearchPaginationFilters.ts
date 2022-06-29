import { useNavigate, useSearch } from '@tanstack/react-location';
import { DefaultLocationGenerics, SearchPagination } from 'eiromplays-ui';
import React from 'react';

import { CustomSearchFilter, SearchFilter } from './SearchFilter';

export type UseSearchPaginationFilters = {
  filter: SearchFilter;
};

export const useSearchPaginationFilters = <
  TGenerics extends DefaultLocationGenerics & {
    Search: {
      pagination: SearchPagination;
      returnUrl: string;
      errorId: string;
      logoutId: string;
      searchFilter: SearchFilter;
    };
  } = DefaultLocationGenerics & {
    Search: {
      pagination: SearchPagination;
      returnUrl: string;
      errorId: string;
      logoutId: string;
      searchFilter: SearchFilter;
    };
  }
>({
  filter,
}: UseSearchPaginationFilters) => {
  const navigate = useNavigate<TGenerics>();
  const { searchFilter: currentFilter } = useSearch<TGenerics>();

  const UpdateOrderBy = React.useCallback(
    (orderByName: string) => {
      navigate({
        search: (old: any) => {
          return {
            ...old,
            searchFilter: {
              ...old?.filter,
              orderBy: old?.filter?.orderBy.include(orderByName)
                ? old?.filter?.orderBy.filter((o: string) => o !== orderByName)
                : [...old.filter.orderBy, orderByName],
            },
          };
        },
        replace: true,
      });
    },
    [currentFilter, navigate]
  );

  const UpdateAdvancedSearchField = React.useCallback(
    (fieldName: string) => {
      navigate({
        search: (old: any) => {
          return {
            ...old,
            searchFilter: {
              ...old?.filter,
              advancedSearch: {
                ...old?.filter?.advancedSearch,
                fields: old?.filter?.advancedSearch?.fields.include(fieldName)
                  ? old?.filter?.advancedSearch?.fields.filter((f: string) => f !== fieldName)
                  : [...old.filter.advancedSearch.fields, fieldName],
              },
            },
          };
        },
        replace: true,
      });
    },
    [currentFilter, navigate]
  );

  const UpdateCustomFilter = React.useCallback(
    (customFilter: CustomSearchFilter) => {
      console.log(customFilter);
      navigate({
        search: (old: any) => {
          return {
            ...old,
            searchFilter: {
              ...old?.searchFilter,
              customFilters: (old?.filter ?? filter).customFilters?.map((cf: CustomSearchFilter) =>
                cf.name === customFilter.name ? customFilter.value : cf.value
              ),
            },
          };
        },
        replace: true,
      });
    },
    [currentFilter, navigate]
  );

  const SetKeyword = React.useCallback(
    (newKeyword: string) => {
      navigate({
        search: (old: any) => {
          return {
            ...old,
            searchFilter: {
              ...old?.filter,
              keyword: newKeyword,
            },
          };
        },
        replace: true,
      });
    },
    [currentFilter, navigate]
  );

  const SetAdvancedSearchKeyword = React.useCallback(
    (newKeyword: string) => {
      navigate({
        search: (old: any) => {
          return {
            ...old,
            searchFilter: {
              ...old?.filter,
              advancedSearch: {
                ...old?.filter?.advancedSearch,
                keyword: newKeyword,
              },
            },
          };
        },
        replace: true,
      });
    },
    [currentFilter, navigate]
  );

  return {
    SetKeyword,
    SetAdvancedSearchKeyword,
    UpdateCustomFilter,
    UpdateOrderBy,
    UpdateAdvancedSearchField,
  };
};
