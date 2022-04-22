import { default as dayjs } from 'dayjs';

export const formatDate = (date: number) => dayjs(date).format('MMMM D, YYYY h:mm A');

// Return text-color that spices up log types
export const formatLogType = (type: string) => {
  switch (type.toLowerCase()) {
    case 'update':
      return 'text-yellow-500 dark:text-yellow-300';
    case 'delete':
      return 'text-red-500 dark:text-red-500';
    case 'create':
      return 'text-green-500 dark:text-green-500';
  }

  return '';
};
