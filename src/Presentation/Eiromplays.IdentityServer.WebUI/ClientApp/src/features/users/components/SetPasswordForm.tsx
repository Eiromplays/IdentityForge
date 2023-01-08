import { Button, Form, InputField } from 'eiromplays-ui';
import * as z from 'zod';

import { SetPasswordDTO, useSetPassword } from '../api/setPassword';

const schema = z
  .object({
    password: z.string(),
    confirmPassword: z.string().min(1, 'Required'),
  })
  .refine((data) => data.confirmPassword === data.password, {
    message: "Passwords don't match",
    path: ['confirmNewPassword'],
  });

type ChangePasswordFormProps = {
  onSuccess: () => void;
};

export const SetPasswordForm = ({ onSuccess }: ChangePasswordFormProps) => {
  const setPasswordMutation = useSetPassword();

  return (
    <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
      <div className="px-4 py-5 sm:px-6">
        <div className="flex justify-between">
          <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
            Set password
          </h3>
        </div>
      </div>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="flex flex-column flex-wrap gap-5 pl-5 pb-5">
          <Form<SetPasswordDTO['data'], typeof schema>
            onSubmit={async (values) => {
              await setPasswordMutation.mutateAsync({ data: values });

              onSuccess();
            }}
            schema={schema}
          >
            {({ register, formState }) => (
              <>
                <InputField
                  label="Password"
                  type="password"
                  error={{
                    errors: formState.errors,
                  }}
                  registration={register('password')}
                />
                <InputField
                  label="Confirm Password"
                  type="password"
                  error={{
                    errors: formState.errors,
                  }}
                  registration={register('confirmPassword')}
                />
                <div>
                  <Button
                    isLoading={setPasswordMutation.isLoading}
                    type="submit"
                    className="w-full mt-4"
                  >
                    Set Password
                  </Button>
                </div>
              </>
            )}
          </Form>
        </div>
      </div>
    </div>
  );
};
