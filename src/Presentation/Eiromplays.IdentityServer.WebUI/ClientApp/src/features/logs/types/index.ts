export type Log = {
  id: string;
  userId: string;
  type: string;
  tableName: string;
  dateTime: number;
  oldValues: string;
  newValues: string;
  affectedColumns: string;
  primaryKey: string;
};
