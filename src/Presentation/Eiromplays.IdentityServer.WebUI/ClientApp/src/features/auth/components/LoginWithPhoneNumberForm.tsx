import { useSearch } from '@tanstack/react-location';
import { Link, Button, InputField, useAuth, CustomInputField, Form } from 'eiromplays-ui';
import React from 'react';
import { isPossiblePhoneNumber } from 'react-phone-number-input';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

import { LocationGenerics } from '@/App';
import { sendVerificationCode } from '@/features/auth';

const schema = z.object({
  rememberMe: z.boolean().optional(),
  code: z.string().optional(),
  login: z.string().min(1, 'Required').refine(isPossiblePhoneNumber, 'Invalid phone number'),
});

type LoginValues = {
  provider: string;
  login: string;
  code: string;
  returnUrl?: string;
  rememberMe?: boolean;
};

export const LoginWithPhoneNumberForm = () => {
  const { login, isLoggingIn } = useAuth();
  const { returnUrl, ReturnUrl } = useSearch<LocationGenerics>();
  const [verificationCodeSent, setVerificationCodeSent] = React.useState<boolean>(false);

  return (
    <div>
      <Form<LoginValues, typeof schema>
        onSubmit={async (values) => {
          if (verificationCodeSent) {
            values.provider = 'Phone';
            values.returnUrl = returnUrl || ReturnUrl;
            await login(values);
            return;
          }

          await sendVerificationCode({ phoneNumber: values.login });
          setVerificationCodeSent(true);
        }}
        schema={schema}
      >
        {({ register, formState, control }) => (
          <>
            <CustomInputField
              label="Phone Number"
              error={formState.errors['login']}
              customInputField={
                <PhoneInputWithCountry
                  className="bg-white dark:bg-gray-900 block px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                  'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                  'dark:focus:border-indigo-900 sm:text-sm"
                  name="login"
                  control={control}
                  register={register('login')}
                />
              }
            />
            {verificationCodeSent && (
              <>
                <InputField
                  type="number"
                  label="Code"
                  error={formState.errors['code']}
                  registration={register('code')}
                />
                <InputField
                  type="checkbox"
                  label="Remember me"
                  error={formState.errors['rememberMe']}
                  registration={register('rememberMe')}
                />
              </>
            )}
            <div>
              <Button isLoading={isLoggingIn} type="submit" className="w-full">
                Log in
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
          <Link to="../register" className="font-medium text-blue-600 hover:text-blue-500">
            Register
          </Link>
        </div>
      </div>
    </div>
  );
};
