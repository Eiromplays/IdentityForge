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
  ignoreAll: boolean;
};
