import { Link, Button, Form, InputField } from 'eiromplays-ui';
import * as z from 'zod';

import {
  ResendEmailConfirmationDTO,
  useResendEmailConfirmation,
} from '../api/resendEmailConfirmation';

const schema = z.object({
  email: z.string().email().min(1, 'Required'),
});

export type ResendEmailConfirmationFormProps = {
  onSuccess?: () => void;
};

export const ResendEmailConfirmationForm = ({ onSuccess }: ResendEmailConfirmationFormProps) => {
  const resendEmailConfirmationMutation = useResendEmailConfirmation();

  return (
    <div>
      <Form<ResendEmailConfirmationDTO, typeof schema>
        onSubmit={async (values) => {
          await resendEmailConfirmationMutation.mutateAsync({ data: values });

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
              <Button
                isLoading={resendEmailConfirmationMutation.isLoading}
                type="submit"
                className="w-full"
              >
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
