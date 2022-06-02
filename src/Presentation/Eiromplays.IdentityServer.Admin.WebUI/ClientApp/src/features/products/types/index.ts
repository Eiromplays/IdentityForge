import { PaginationFilter } from 'eiromplays-ui';

import { Brand } from '@/features/brands';

export type Product = {
  id: string;
  name: string;
  description: string;
  rate: number;
  imagePath: string;
  brandId: string;
  brandName: string;
};

export type ProductDetails = {
  id: string;
  name: string;
  description: string;
  rate: number;
  imagePath: string;
  brand: Brand;
};

export type SearchProductDTO = PaginationFilter;
