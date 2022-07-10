import { Link, Button, Form, InputField, CustomInputField } from 'eiromplays-ui';
import * as React from 'react';
import { isPossiblePhoneNumber } from 'react-phone-number-input';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

import { useExternalLoginConfirmation } from '../api/externalLoginConfirmation';
import { RegisterDto } from '../components/RegisterForm';

const schema = z
  .object({
    firstName: z.string().min(1, 'Required'),
    lastName: z.string().min(1, 'Required'),
    email: z.string().min(1, 'Required'),
    userName: z.string().min(1, 'Required'),
    password: z.string().nullable(),
    confirmPassword: z.string().nullable(),
    phoneNumber: z
      .string()
      .nullable()
      .refine((v) => (v ? isPossiblePhoneNumber(v) : true), 'Invalid phone number'),
  })
  .refine((data) => data.confirmPassword === data.password, {
    message: "Passwords don't match",
    path: ['confirmPassword'],
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
      <Form<RegisterDto, typeof schema>
        onSubmit={async (values) => {
          values.provider = 'external';
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
        {({ register, formState, control }) => (
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
            <CustomInputField
              label="Phone Number"
              error={formState.errors['phoneNumber']}
              customInputField={
                <PhoneInputWithCountry
                  className="bg-white dark:bg-gray-900 block px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                  'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                  'dark:focus:border-indigo-900 sm:text-sm"
                  name="phoneNumber"
                  control={control}
                  register={register('phoneNumber')}
                />
              }
            />
            <InputField
              type="password"
              label="Password"
              error={formState.errors['password']}
              registration={register('password')}
            />
            <InputField
              type="password"
              label="Confirm Password"
              error={formState.errors['confirmPassword']}
              registration={register('confirmPassword')}
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
