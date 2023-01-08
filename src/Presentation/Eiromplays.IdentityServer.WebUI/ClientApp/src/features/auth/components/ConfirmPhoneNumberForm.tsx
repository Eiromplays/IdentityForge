import { useSearch } from '@tanstack/react-location';
import { Link, Button, Form, InputField } from 'eiromplays-ui';
import React from 'react';
import * as z from 'zod';

import { LocationGenerics } from '@/App';

import { ConfirmPhoneNumberDTO, useConfirmPhoneNUmber } from '../api/confirmPhoneNumber';

const schema = z.object({
  code: z.string().optional(),
});

export const ConfirmPhoneNumberForm = () => {
  const { userId } = useSearch<LocationGenerics>();

  const confirmPhoneNumberMutation = useConfirmPhoneNUmber();

  if (!userId) return null;

  return (
    <div>
      <Form<ConfirmPhoneNumberDTO, typeof schema>
        onSubmit={async (values) => {
          values.userId = userId || '';
          await confirmPhoneNumberMutation.mutateAsync(values);
        }}
        schema={schema}
      >
        {({ register, formState }) => (
          <>
            <InputField
              type="number"
              label="Code"
              error={{
                errors: formState.errors,
              }}
              registration={register('code')}
            />
            <div>
              <Button
                isLoading={confirmPhoneNumberMutation.isLoading}
                type="submit"
                className="w-full"
              >
                Verify
              </Button>
            </div>
          </>
        )}
      </Form>
      <div className="mt-2 gap-5 flex items-center justify-center">
        <div className="text-sm">
          <Link to="../forgot-password" className="font-medium text-blue-600 hover:text-blue-500">
            Forgot password?
          </Link>
        </div>
        <div className="text-sm">
          <Link to="../login" className="font-medium text-blue-600 hover:text-blue-500">
            Login
          </Link>
        </div>
      </div>
    </div>
  );
};
