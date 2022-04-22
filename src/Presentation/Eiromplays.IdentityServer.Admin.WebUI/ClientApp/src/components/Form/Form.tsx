import { zodResolver } from '@hookform/resolvers/zod';
import clsx from 'clsx';
import * as React from 'react';
import { useForm, UseFormReturn, SubmitHandler, UseFormProps } from 'react-hook-form';
import { ZodType, ZodTypeDef } from 'zod';

type FormProps<TFormValues, Schema> = {
  className?: string;
  onSubmit: SubmitHandler<TFormValues>;
  children: (methods: UseFormReturn<TFormValues, any>) => React.ReactNode;
  options?: UseFormProps<TFormValues>;
  id?: string;
  schema?: Schema;
  onChange?: (items: [string, unknown][], files: FileList[]) => void;
  files?: FileList[];
};

// Return a array with FileList's using watch method and checking if it is a file
export const Form = <
  TFormValues extends Record<string, unknown> = Record<string, unknown>,
  Schema extends ZodType<unknown, ZodTypeDef, unknown> = ZodType<unknown, ZodTypeDef, unknown>
>({
  onSubmit,
  children,
  className,
  options,
  id,
  schema,
  onChange,
}: FormProps<TFormValues, Schema>) => {
  const methods = useForm<TFormValues>({ ...options, resolver: schema && zodResolver(schema) });

  const filesLists: FileList[] = [];
  const items: [string, unknown][] = [];
  Object.entries(methods.watch()).forEach((item) => {
    items.push(item);

    if (item[1] instanceof FileList) {
      filesLists.push(item[1]);
    }
  });

  onChange && onChange(items, filesLists);

  return (
    <form className={clsx('space-y-', className)} onSubmit={methods.handleSubmit(onSubmit)} id={id}>
      {children(methods)}
    </form>
  );
};
