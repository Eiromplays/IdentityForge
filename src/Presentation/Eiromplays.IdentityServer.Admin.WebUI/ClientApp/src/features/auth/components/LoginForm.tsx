import { Link } from 'react-router-dom';
import * as z from 'zod';

import { Button } from '@/components/Elements';
import { Form, InputField } from '@/components/Form';
import { useAuth } from '@/lib/auth';

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

type LoginFormProps = {
  onSuccess: () => void;
};

export const LoginForm = ({ onSuccess }: LoginFormProps) => {
  const { login, isLoggingIn } = useAuth();

  return (
    <div>
      <Form<LoginValues, typeof schema>
        onSubmit={async (values) => {
          //TODO: Find a better way to get the returnUrl
          let returnUrl = '';
          const idx = location.href.toLowerCase().indexOf('?returnurl=');
          if (idx > 0) {
            returnUrl = location.href.substring(idx + 11);
          }

          values.returnUrl = returnUrl;
          const response = await login(values);

          if (response) onSuccess();
        }}
        schema={schema}
      >
        {({ register, formState }) => (
          <>
            <InputField
              label="Login / Email, Username, Phone Number or User ID"
              error={formState.errors['login']}
              registration={register('login')}
            />
            <InputField
              type="password"
              label="Password"
              error={formState.errors['password']}
              registration={register('password')}
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
