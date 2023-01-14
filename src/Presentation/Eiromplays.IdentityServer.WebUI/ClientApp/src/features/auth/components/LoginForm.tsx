import { useSearch } from '@tanstack/react-location';
import { Link, Button, Form, InputField, useAuth } from 'eiromplays-ui';
import * as z from 'zod';

import { LocationGenerics } from '@/App';

const schema = z.object({
  login: z.string().min(1, 'Required'),
  password: z.string().min(1, 'Required'),
  rememberMe: z.boolean(),
});

type LoginValues = {
  login: string;
  password: string;
  returnUrl?: string;
  rememberMe?: boolean;
};

export const LoginForm = () => {
  const { login, isLoggingIn } = useAuth();
  const { returnUrl, ReturnUrl } = useSearch<LocationGenerics>();

  // Hacky way to redirect to the app if the user is already logged in
  // Or if there is no returnUrl in the url
  const redirectUrl = returnUrl || ReturnUrl;

  if (!redirectUrl) {
    window.location.href = '/bff/login?returnUrl=/app';

    return null;
  }

  return (
    <div>
      <Form<LoginValues, typeof schema>
        onSubmit={async (values) => {
          values.returnUrl = redirectUrl;
          await login(values);
        }}
        schema={schema}
      >
        {({ register, formState }) => (
          <>
            <InputField
              label="Login / Email, Username, Phone Number or User ID"
              autoComplete="username"
              error={{
                errors: formState.errors,
              }}
              registration={register('login')}
            />
            <InputField
              type="password"
              label="Password"
              autoComplete="current-password"
              error={{
                errors: formState.errors,
              }}
              registration={register('password')}
            />
            <InputField
              type="checkbox"
              label="Remember me"
              error={{
                errors: formState.errors,
              }}
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
      <div className="mt-2 gap-5 flex flex-wrap items-center justify-center">
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
        <div className="text-sm">
          <Link
            to="../resend-email-confirmation"
            className="font-medium text-blue-600 hover:text-blue-500"
          >
            Resend email confirmation
          </Link>
        </div>
      </div>
    </div>
  );
};
