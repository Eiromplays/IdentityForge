export type BaseEntity = {
  id: string;
  createdAt: number;
};

export type Claim = {
  type: string;
  value: string;
};

export type WhitelistAxiosError = {
  status: number;
  urls: string[];
  ignoreAll?: boolean;
};

export type MessageResponse = {
  message: string;
};

export type PaginationFilter = BaseFilter & {
  pageNumber: number;
  pageSize: number;
  orderBy?: string[];
};

export type BaseFilter = {
  advancedSearch?: Search;
  keyword?: string;
};

export type Search = {
  fields: string[];
  keyword?: string;
};

export type PaginationResponse<T> = {
  data: T[];
  currentPage: number;
  totalPages: number;
  totalCount: number;
  pageSize: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
};
