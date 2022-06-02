import { Link, Button, Form, InputField } from 'eiromplays-ui';
import * as React from 'react';
import * as z from 'zod';

import {
  ExternalLoginConfirmationViewModel,
  useExternalLoginConfirmation,
} from '../api/externalLoginConfirmation';

const schema = z.object({
  firstName: z.string().min(1, 'Required'),
  lastName: z.string().min(1, 'Required'),
  email: z.string().min(1, 'Required'),
  userName: z.string().min(1, 'Required'),
  phoneNumber: z.string().nullable(),
});

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
      <Form<ExternalLoginConfirmationViewModel, typeof schema>
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
              type="text"
              label="First Name"
              error={formState.errors['firstName']}
              registration={register('firstName')}
            />
            <InputField
              type="text"
              label="Last Name"
              error={formState.errors['lastName']}
              registration={register('lastName')}
            />
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
            <InputField
              type="tel"
              label="Phone number"
              error={formState.errors['phoneNumber']}
              registration={register('phoneNumber')}
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
          <Link to="/auth/login" className="font-medium text-blue-600 hover:text-blue-500">
            Log In
          </Link>
        </div>
      </div>
    </div>
  );
};
