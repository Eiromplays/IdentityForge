import { useSearch } from '@tanstack/react-location';
import { Link, Button, Form, InputField, useAuth } from 'eiromplays-ui';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

import { LocationGenerics } from '@/App';

const schema = z.object({
  login: z.string().min(1, 'Required'),
  rememberMe: z.boolean(),
});

type LoginValues = {
  login: string;
  returnUrl?: string;
  rememberMe?: boolean;
};

export const LoginWithPhoneNumberForm = () => {
  const { login, isLoggingIn } = useAuth();
  const { returnUrl, ReturnUrl } = useSearch<LocationGenerics>();

  return (
    <div>
      <Form<LoginValues, typeof schema>
        onSubmit={async (values) => {
          values.returnUrl = returnUrl || ReturnUrl;
          await login(values);
        }}
        schema={schema}
      >
        {({ register, formState, control }) => (
          <>
            <p>Phone number:</p>
            <PhoneInputWithCountry
              className="bg-white dark:bg-gray-900 block px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                  'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                  'dark:focus:border-indigo-900 sm:text-sm"
              name="login"
              control={control}
            />
            <InputField
              type="checkbox"
              label="Remember me"
              error={formState.errors['rememberMe']}
              registration={register('rememberMe')}
            />
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
