export type DashboardStats = {
  productCount: number;
  brandCount: number;
  userCount: number;
  roleCount: number;
  clientCount: number;
  identityResourceCount: number;
  apiResourceCount: number;
  apiScopeCount: number;
  DataEnterBarChart: ChartSeries[];
};

export type ChartSeries = {
  name: string;
  data: number[];
};
