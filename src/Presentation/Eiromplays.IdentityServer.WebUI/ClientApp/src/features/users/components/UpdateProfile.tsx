import { PencilIcon } from '@heroicons/react/solid';
import * as z from 'zod';

import { Button } from '@/components/Elements';
import { Form, FormDrawer, InputField } from '@/components/Form';
import { useAuth } from '@/lib/auth';

import { UpdateProfileDTO, useUpdateProfile } from '../api/updateProfile';

const schema = z.object({
  username: z.string().min(1, 'Required'),
  email: z.string().min(1, 'Required'),
});

export const UpdateProfile = () => {
  const { user } = useAuth();
  const updateProfileMutation = useUpdateProfile();

  return (
    <FormDrawer
      isDone={updateProfileMutation.isSuccess}
      triggerButton={
        <Button startIcon={<PencilIcon className="h-4 w-4" />} size="sm">
          Update Profile
        </Button>
      }
      title="Update Profile"
      submitButton={
        <Button
          form="update-profile"
          type="submit"
          size="sm"
          isLoading={updateProfileMutation.isLoading}
        >
          Submit
        </Button>
      }
    >
      <Form<UpdateProfileDTO['data'], typeof schema>
        id="update-profile"
        onSubmit={async (values) => {
          await updateProfileMutation.mutateAsync({ id: user?.id, data: values });
        }}
        options={{
          defaultValues: {
            username: user?.username,
            email: user?.email,
          },
        }}
        schema={schema}
      >
        {({ register, formState }) => (
          <>
            <InputField
              label="Username"
              error={formState.errors['username']}
              registration={register('username')}
            />
            <InputField
              label="Email Address"
              type="email"
              error={formState.errors['email']}
              registration={register('email')}
            />
          </>
        )}
      </Form>
    </FormDrawer>
  );
};
