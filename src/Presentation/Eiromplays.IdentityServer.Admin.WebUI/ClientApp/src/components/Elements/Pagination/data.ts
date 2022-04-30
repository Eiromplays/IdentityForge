export interface PaginationPageSizeOption {
  readonly value: number;
  readonly label: string;
  readonly isFixed?: boolean;
  readonly isDisabled?: boolean;
}

export const paginationPageSizeOptions: readonly PaginationPageSizeOption[] = [
  { value: 5, label: '5', isFixed: true },
  { value: 10, label: '10', isFixed: true },
  { value: 20, label: '20', isFixed: true },
  { value: 50, label: '50', isFixed: true },
  { value: 100, label: '100', isFixed: true },
  { value: 200, label: '200', isFixed: true },
  { value: 500, label: '500', isFixed: true },
  { value: 1000, label: '1000', isFixed: true },
  { value: 2000, label: '2000', isFixed: true },
  { value: 5000, label: '5000', isFixed: true },
];

export const defaultPaginationPageSizeOption: PaginationPageSizeOption = {
  value: 10,
  label: '10',
  isFixed: true,
};
