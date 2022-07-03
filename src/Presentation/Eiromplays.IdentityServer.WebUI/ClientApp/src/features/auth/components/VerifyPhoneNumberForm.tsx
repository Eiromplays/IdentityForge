import { useSearch } from '@tanstack/react-location';
import { Link, Button, Form, InputField } from 'eiromplays-ui';
import React from 'react';
import { toast } from 'react-toastify';
import * as z from 'zod';

import { LocationGenerics } from '@/App';

import { useVerifyPhoneNumber, VerifyPhoneNumberDTO } from '../api/verifyPhoneNumber';

const schema = z.object({
  code: z.string().optional(),
});

export const VerifyPhoneNumberForm = () => {
  const { returnUrl, ReturnUrl, userId } = useSearch<LocationGenerics>();
  console.log(returnUrl || ReturnUrl);

  const verifyPhoneNumberMutation = useVerifyPhoneNumber();

  if (!userId) return null;

  return (
    <div>
      <Form<VerifyPhoneNumberDTO, typeof schema>
        onSubmit={async (values) => {
          values.userId = userId || '';
          const response = await verifyPhoneNumberMutation.mutateAsync(values);
          toast.success(response.message);
        }}
        schema={schema}
      >
        {({ register, formState }) => (
          <>
            <InputField
              type="number"
              label="Code"
              error={formState.errors['code']}
              registration={register('code')}
            />
            <div>
              <Button
                isLoading={verifyPhoneNumberMutation.isLoading}
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
