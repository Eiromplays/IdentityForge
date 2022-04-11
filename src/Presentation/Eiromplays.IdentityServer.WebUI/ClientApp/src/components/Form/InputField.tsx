import clsx from 'clsx';
import { UseFormRegisterReturn } from 'react-hook-form';

import { FieldWrapper, FieldWrapperPassThroughProps } from './FieldWrapper';

type InputFieldProps = FieldWrapperPassThroughProps & {
  type?: 'text' | 'email' | 'password' | 'file' | 'checkbox';
  className?: string;
  multiple?: boolean;
  accept?: string;
  registration: Partial<UseFormRegisterReturn>;
};

export const InputField = (props: InputFieldProps) => {
  const { type = 'text', label, className, multiple, accept, registration, error } = props;

  return (
    <FieldWrapper label={label} error={error}>
      <input
        type={type}
        multiple={multiple}
        accept={accept}
        className={
          type !== 'checkbox'
            ? clsx(
                'bg-white dark:bg-gray-900 appearance-none block w-full px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                'dark:focus:border-indigo-900 sm:text-sm',
                className
              )
            : clsx(
                'bg-white dark:bg-gray-900 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                'dark:focus:border-indigo-900 sm:text-sm',
                className
              )
        }
        {...registration}
      />
    </FieldWrapper>
  );
};
