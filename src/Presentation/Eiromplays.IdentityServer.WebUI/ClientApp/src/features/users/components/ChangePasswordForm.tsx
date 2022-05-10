import { Button, Form, InputField } from 'eiromplays-ui';
import * as z from 'zod';

import { ChangePasswordDTO, useChangePassword } from '../api/changePassword';

const schema = z
  .object({
    password: z.string().min(1, 'Required'),
    newPassword: z.string().min(1, 'Required'),
    confirmNewPassword: z.string().min(1, 'Required'),
  })
  .refine((data) => data.confirmNewPassword === data.newPassword, {
    message: "Passwords don't match",
    path: ['confirmNewPassword'],
  })
  .refine((data) => data.newPassword === data.password, {
    message: "New Password can't be the same as the old one",
    path: ['newPassword'],
  });

type ChangePasswordFormProps = {
  onSuccess: () => void;
};

export const ChangePasswordForm = ({ onSuccess }: ChangePasswordFormProps) => {
  const changePasswordMutation = useChangePassword();

  return (
    <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
      <div className="flex flex-column flex-wrap gap-5 pl-5 pb-5">
        <Form<ChangePasswordDTO['data'], typeof schema>
          onSubmit={async (values) => {
            await changePasswordMutation.mutateAsync({ data: values });

            onSuccess();
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                type="password"
                label="Current Password"
                error={formState.errors['password']}
                registration={register('password')}
              />
              <InputField
                type="password"
                label="New Password"
                error={formState.errors['newPassword']}
                registration={register('newPassword')}
              />
              <InputField
                type="password"
                label="Confirm New Password"
                error={formState.errors['confirmNewPassword']}
                registration={register('confirmNewPassword')}
              />
              <div>
                <Button
                  isLoading={changePasswordMutation.isLoading}
                  type="submit"
                  className="w-full mt-4"
                >
                  Change Password
                </Button>
              </div>
            </>
          )}
        </Form>
      </div>
    </div>
  );
};
