export type Claim = {
  type: string;
  value: string;
};

export type WhitelistAxiosError = {
  status: number;
  urls: string[];
  ignoreAll: boolean;
};
