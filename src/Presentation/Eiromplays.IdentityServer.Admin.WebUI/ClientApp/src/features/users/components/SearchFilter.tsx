import {
  Form,
  InputField,
  DefaultLocationGenerics,
  Search,
  SearchPagination,
  Button,
} from 'eiromplays-ui';
import React from 'react';

import {
  UseSearchPaginationFilters,
  useSearchPaginationFilters,
} from './useSearchPaginationFilters';

export type SearchFiltersProps = UseSearchPaginationFilters;

export type CustomSearchFilter = {
  name: string;
  value: any;
  formType:
    | 'text'
    | 'email'
    | 'password'
    | 'file'
    | 'checkbox'
    | 'hidden'
    | 'tel'
    | 'number'
    | 'radio'
    | 'range'
    | 'search'
    | 'url';
};

export type SearchFilter = {
  customFilters: CustomSearchFilter[];
  orderBy: string[];
  advancedSearch: Search;
  keyword: string;
};

export const SearchFilter = <
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
}: SearchFiltersProps) => {
  const {
    UpdateAdvancedSearchField,
    UpdateOrderBy,
    SetAdvancedSearchKeyword,
    SetKeyword,
    UpdateCustomFilter,
  } = useSearchPaginationFilters<TGenerics>({
    filter: filter,
  });

  return (
    <div>
      <Form onSubmit={async () => {}}>
        {({ register }) =>
          filter.customFilters?.map((customFilter, index) => (
            <InputField
              key={index}
              type={customFilter.formType}
              label={customFilter.name}
              //error={formState.errors['username']}
              registration={register(customFilter.name, {
                onChange: (value) => {
                  customFilter.value = value?.target?.checked;
                  UpdateCustomFilter(customFilter);
                },
              })}
            />
          ))
        }
      </Form>
    </div>
  );
};
