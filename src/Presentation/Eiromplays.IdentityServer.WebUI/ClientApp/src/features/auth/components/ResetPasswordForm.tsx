import { useSearch } from '@tanstack/react-location';
import { Link, Button, Form, InputField } from 'eiromplays-ui';
import * as z from 'zod';

import { LocationGenerics } from '@/App';

import { ResetPasswordDTO, useResetPassword } from '../api/resetPassword';

const schema = z.object({
  email: z.string().email().min(1, 'Required'),
  password: z.string().min(1, 'Required'),
});

export type ResetPasswordFormProps = {
  onSuccess?: () => void;
};

export const ResetPasswordForm = ({ onSuccess }: ResetPasswordFormProps) => {
  const { token } = useSearch<LocationGenerics>();
  const resetPasswordMutation = useResetPassword();

  return (
    <div>
      <Form<ResetPasswordDTO, typeof schema>
        onSubmit={async (values) => {
          values.token = token ?? '';
          await resetPasswordMutation.mutateAsync({ data: values });

          if (onSuccess) onSuccess();
        }}
        schema={schema}
      >
        {({ register, formState }) => (
          <>
            <InputField
              type="email"
              label="Email"
              error={formState.errors['email']}
              registration={register('email')}
            />
            <InputField
              type="password"
              label="Password"
              error={formState.errors['password']}
              registration={register('password')}
            />
            <div>
              <Button isLoading={resetPasswordMutation.isLoading} type="submit" className="w-full">
                Submit
              </Button>
            </div>
          </>
        )}
      </Form>
      <div className="mt-2 gap-5 flex items-center justify-center">
        <div className="text-sm">
          <Link to="../login" className="font-medium text-blue-600 hover:text-blue-500">
            Login
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
