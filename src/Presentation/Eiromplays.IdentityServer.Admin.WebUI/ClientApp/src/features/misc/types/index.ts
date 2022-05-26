export type DashboardStats = {
  productCount: number;
  brandCount: number;
  userCount: number;
  roleCount: number;
  DataEnterBarChart: ChartSeries[];
};

export type ChartSeries = {
  name: string;
  data: number[];
};
