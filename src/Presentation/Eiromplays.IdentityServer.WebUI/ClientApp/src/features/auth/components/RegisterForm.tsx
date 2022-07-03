import { Link, Button, Form, InputField, useAuth, CustomInputField } from 'eiromplays-ui';
import React from 'react';
import { isPossiblePhoneNumber } from 'react-phone-number-input';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

const schema = z
  .object({
    userName: z.string().min(1, 'Required'),
    email: z.string().min(1, 'Required'),
    firstName: z.string().min(1, 'Required'),
    lastName: z.string().min(1, 'Required'),
    password: z.string().min(1, 'Required'),
    confirmPassword: z.string().min(1, 'Required'),
    phoneNumber: z
      .string()
      .optional()
      .refine((v) => (v ? isPossiblePhoneNumber(v) : true), 'Invalid phone number'),
  })
  .refine((data) => data.confirmPassword === data.password, {
    message: "Passwords don't match",
    path: ['confirmPassword'],
  });

type RegisterValues = {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber: string;
  password: string;
  confirmPassword: string;
};

type RegisterFormProps = {
  onSuccess: () => void;
};

export const RegisterForm = ({ onSuccess }: RegisterFormProps) => {
  const { register, isRegistering } = useAuth();

  return (
    <div>
      <Form<RegisterValues, typeof schema>
        onSubmit={async (values) => {
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
              type="text"
              label="Username"
              error={formState.errors['userName']}
              registration={register('userName')}
            />
            <InputField
              type="email"
              label="Email Address"
              error={formState.errors['email']}
              registration={register('email')}
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
