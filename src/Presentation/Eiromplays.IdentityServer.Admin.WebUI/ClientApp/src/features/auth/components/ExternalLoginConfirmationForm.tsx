import * as React from 'react';
import { Link } from 'react-router-dom';
import * as z from 'zod';

import { Button } from '@/components/Elements';
import { Form, InputField } from '@/components/Form';

import { useExternalLoginConfirmation } from '../api/externalLoginConfirmation';

const schema = z.object({
  email: z.string().min(1, 'Required'),
  userName: z.string().min(1, 'Required'),
});

type RegisterValues = {
  email: string;
  userName: string;
};

type ExternalLoginConfirmationFormProps = {
  email?: string;
  userName?: string;
  returnUrl?: string;
};

export const ExternalLoginConfirmationForm = ({
  email,
  userName,
  returnUrl,
}: ExternalLoginConfirmationFormProps) => {
  const externalLoginConfirmationMutation = useExternalLoginConfirmation();

  return (
    <div>
      <Form<RegisterValues, typeof schema>
        onSubmit={async (values) => {
          await externalLoginConfirmationMutation.mutateAsync({
            data: values,
            returnUrl: returnUrl,
          });
        }}
        schema={schema}
        options={{
          defaultValues: {
            email: email,
            userName: userName,
          },
          shouldUnregister: true,
        }}
      >
        {({ register, formState }) => (
          <>
            <InputField
              type="email"
              label="Email Address"
              error={formState.errors['email']}
              registration={register('email')}
            />
            <InputField
              type="text"
              label="Username"
              error={formState.errors['userName']}
              registration={register('userName')}
            />

            <div>
              <Button
                isLoading={externalLoginConfirmationMutation.isLoading}
                type="submit"
                className="w-full"
              >
                Register
              </Button>
            </div>
          </>
        )}
      </Form>
      <div className="mt-2 flex items-center justify-end">
        <div className="text-sm">
          <Link to="../login" className="font-medium text-blue-600 hover:text-blue-500">
            Log In
          </Link>
        </div>
      </div>
    </div>
  );
};
