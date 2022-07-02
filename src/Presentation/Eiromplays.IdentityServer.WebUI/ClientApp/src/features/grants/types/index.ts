export type Grant = {
  id: string;
  clientId: string;
  clientName: string;
  clientLogoUrl?: string;
  description?: string;
  created: number;
  expires: number;
  identityGrantNames: string[];
  apiGrantNames: string[];
};
