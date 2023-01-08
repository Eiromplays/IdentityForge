import { Link, Button, Form, InputField } from 'eiromplays-ui';
import * as z from 'zod';

import { useForgotPassword } from '../api/forgotPassword';

const schema = z.object({
  email: z.string().email().min(1, 'Required'),
});

type ForgotPasswordValues = {
  email: string;
};

type ForgotPasswordFormProps = {
  onSuccess?: () => void;
};

export const ForgotPasswordForm = ({ onSuccess }: ForgotPasswordFormProps) => {
  const forgotPasswordMutation = useForgotPassword();

  return (
    <div>
      <Form<ForgotPasswordValues, typeof schema>
        onSubmit={async (values) => {
          await forgotPasswordMutation.mutateAsync({ data: values });

          if (onSuccess) onSuccess();
        }}
        schema={schema}
      >
        {({ register, formState }) => (
          <>
            <InputField
              type="email"
              label="Email"
              error={{
                errors: formState.errors,
              }}
              registration={register('email')}
            />
            <div>
              <Button isLoading={forgotPasswordMutation.isLoading} type="submit" className="w-full">
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
