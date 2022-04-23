import clsx from 'clsx';
import { UseFormRegisterReturn } from 'react-hook-form';

import { FieldWrapper, FieldWrapperPassThroughProps } from './FieldWrapper';

type InputFieldProps = FieldWrapperPassThroughProps & {
  type?: 'text' | 'email' | 'password' | 'file' | 'checkbox' | 'hidden';
  className?: string;
  multiple?: boolean;
  accept?: string;
  value?: string | number | readonly string[] | undefined;
  placeholder?: string;
  disabled?: boolean;
  registration: Partial<UseFormRegisterReturn>;
};

export const InputField = (props: InputFieldProps) => {
  const {
    type = 'text',
    label,
    subLabel,
    className,
    multiple,
    accept,
    value,
    placeholder,
    disabled = false,
    registration,
    error,
  } = props;

  return (
    <FieldWrapper label={label} subLabel={subLabel} error={error}>
      <fieldset disabled={disabled}>
        <input
          type={type}
          multiple={multiple}
          accept={accept}
          value={value}
          placeholder={placeholder}
          className={
            type !== 'checkbox'
              ? clsx(
                  'bg-white dark:bg-gray-900 appearance-none block w-full px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                  'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                  'dark:focus:border-indigo-900 sm:text-sm',
                  className
                )
              : clsx(
                  'bg-white dark:bg-gray-900 block px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                  'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                  'dark:focus:border-indigo-900 sm:text-sm',
                  className
                )
          }
          {...registration}
        />
      </fieldset>
    </FieldWrapper>
  );
};
