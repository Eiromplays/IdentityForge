import { Link, Button, Form, InputField, useAuth, CustomInputField } from 'eiromplays-ui';
import React from 'react';
import { isPossiblePhoneNumber } from 'react-phone-number-input';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

const schema = z.object({
  email: z.string().min(1, 'Required'),
  userName: z.string().min(1, 'Required'),
  firstName: z.string().min(1, 'Required'),
  lastName: z.string().min(1, 'Required'),
  phoneNumber: z.string().min(1, 'Required').refine(isPossiblePhoneNumber, 'Invalid phone number'),
  agreement: z.boolean().refine((v) => v, 'You must agree to the terms and conditions'),
});

type RegisterValues = {
  provider: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber: string;
  agreement: boolean;
};

type RegisterUsingPhoneNumberFormProps = {
  onSuccess: () => void;
};

export const RegisterUsingPhoneNumberForm = ({ onSuccess }: RegisterUsingPhoneNumberFormProps) => {
  const { register, isRegistering } = useAuth();

  return (
    <div>
      <Form<RegisterValues, typeof schema>
        onSubmit={async (values) => {
          values.provider = 'Phone';
          await register(values);

          onSuccess();
        }}
        schema={schema}
        options={{
          shouldUnregister: true,
        }}
      >
        {({ register, formState, control }) => (
          <>
            <InputField
              type="text"
              label="First Name"
              error={{
                errors: formState.errors,
              }}
              registration={register('firstName')}
            />
            <InputField
              type="text"
              label="Last Name"
              error={{
                errors: formState.errors,
              }}
              registration={register('lastName')}
            />
            <InputField
              type="text"
              label="Username"
              error={{
                errors: formState.errors,
              }}
              registration={register('userName')}
            />
            <InputField
              type="email"
              label="Email Address"
              error={{
                errors: formState.errors,
              }}
              registration={register('email')}
            />
            <CustomInputField
              label="Phone Number"
              error={{
                name: 'phoneNumber',
                errors: formState.errors,
              }}
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
              type="checkbox"
              label="Agree to terms and conditions"
              error={{
                errors: formState.errors,
              }}
              registration={register('agreement')}
            />
            <div>
              <Button isLoading={isRegistering} type="submit" className="w-full">
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
