import { Button, ConfirmationDialog, Form, InputField, useAuth } from 'eiromplays-ui';
import * as z from 'zod';

import { ChangePasswordDTO, useChangePassword } from '../api/changePassword';

const schema = z
  .object({
    password: z.string(),
    newPassword: z.string().min(1, 'Required'),
    confirmNewPassword: z.string().min(1, 'Required'),
  })
  .refine((data) => data.confirmNewPassword === data.newPassword, {
    message: "Passwords don't match",
    path: ['confirmNewPassword'],
  })
  .refine((data) => data.newPassword !== data.password, {
    message: "New Password can't be the same as the old one",
    path: ['newPassword'],
  });

type ChangePasswordFormProps = {
  onSuccess: () => void;
};

export const ChangePasswordForm = ({ onSuccess }: ChangePasswordFormProps) => {
  const { user } = useAuth();
  const changePasswordMutation = useChangePassword();

  return (
    <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
      <div className="flex flex-column flex-wrap gap-5 pl-5 pb-5">
        <Form<ChangePasswordDTO['data'], typeof schema>
          id="change-password"
          onSubmit={async (values) => {
            values.userId = user?.id;
            await changePasswordMutation.mutateAsync({ data: values });

            onSuccess();
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="Current Password"
                type="password"
                error={formState.errors['password']}
                registration={register('password')}
              />
              <InputField
                label="New Password"
                type="password"
                error={formState.errors['newPassword']}
                registration={register('newPassword')}
              />
              <InputField
                label="Confirm New Password"
                type="password"
                error={formState.errors['confirmNewPassword']}
                registration={register('confirmNewPassword')}
              />
              <div className="mt-4">
                <ConfirmationDialog
                  icon="warning"
                  title="Change Password"
                  body="Are you sure you want to update your password? This will log you out."
                  triggerButton={
                    <Button size="sm" isLoading={changePasswordMutation.isLoading}>
                      Change Password
                    </Button>
                  }
                  confirmButton={
                    <Button
                      form="change-password"
                      type="submit"
                      className="mt-2"
                      variant="warning"
                      size="sm"
                      isLoading={changePasswordMutation.isLoading}
                    >
                      Submit
                    </Button>
                  }
                />
              </div>
            </>
          )}
        </Form>
      </div>
    </div>
  );
};
