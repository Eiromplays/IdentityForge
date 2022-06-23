import { Link, Button, Form, InputField, useAuth } from 'eiromplays-ui';
import * as z from 'zod';

const schema = z.object({
  twoFactorCode: z.string().max(7, 'Required').min(6, 'Required'),
  rememberMachine: z.boolean(),
});

type LoginValues = {
  twoFactorCode: string;
  rememberMachine: boolean;
  returnUrl?: string;
  rememberMe: boolean;
};

type LoginFormProps = {
  returnUrl?: string;
  rememberMe: boolean;
  onSuccess?: () => void;
};

export const Login2faForm = ({ rememberMe, returnUrl, onSuccess }: LoginFormProps) => {
  const { login2fa, isLoggingIn } = useAuth();

  return (
    <div>
      <Form<LoginValues, typeof schema>
        onSubmit={async (values) => {
          values.returnUrl = returnUrl;
          values.rememberMe = rememberMe;
          const response = await login2fa(values);

          if (response) onSuccess?.();
        }}
        schema={schema}
      >
        {({ register, formState }) => (
          <>
            <InputField
              label="Two-factor code"
              error={formState.errors['twoFactorCode']}
              registration={register('twoFactorCode')}
            />
            <InputField
              type="checkbox"
              label="Remember"
              error={formState.errors['rememberMachine']}
              registration={register('rememberMachine')}
            />
            <div>
              <Button isLoading={isLoggingIn} type="submit" className="w-full">
                Log in
              </Button>
            </div>
          </>
        )}
      </Form>
      <div className="mt-2 flex items-center justify-end">
        <div className="text-sm">
          <Link to="../register" className="font-medium text-blue-600 hover:text-blue-500">
            Register
          </Link>
        </div>
      </div>
    </div>
  );
};
