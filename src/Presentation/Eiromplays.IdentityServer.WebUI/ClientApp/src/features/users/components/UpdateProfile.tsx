import { PencilIcon, TrashIcon } from '@heroicons/react/solid';
import * as z from 'zod';

import { Button } from '@/components/Elements';
import { Form, FormDrawer, InputField } from '@/components/Form';
import { useAuth } from '@/lib/auth';

import { UpdateProfileDTO, useUpdateProfile } from '../api/updateProfile';

const schema = z.object({
  username: z.string().min(1, 'Required'),
  email: z.string().min(1, 'Required'),
  gravatarEmail: z.string(),
});

export const UpdateProfile = () => {
  const { user } = useAuth();
  let files: FileList[] = [];
  const { updateProfileMutation, deleteProfilePictureMutation } = useUpdateProfile();

  return (
    <>
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
            const answer = window.confirm(
              'Are you sure you want to update your user? This will log you out. And you will be required to log back in to update your stored information.'
            );

            if (!answer) return;
            values.profilePicture = files[0][0];
            await updateProfileMutation.mutateAsync({ id: user?.id, data: values });
          }}
          options={{
            defaultValues: {
              username: user?.username,
              email: user?.email,
              gravatarEmail: user?.gravatarEmail,
            },
          }}
          schema={schema}
          onChange={(_, file) => (files = file)}
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
              <InputField
                label="Gravatar Email Address"
                type="email"
                error={formState.errors['gravatarEmail']}
                registration={register('gravatarEmail')}
              />
              <InputField
                label="Profile Picture"
                type="file"
                accept={'image/*'}
                error={formState.errors['profilePicture']}
                registration={register('profilePicture')}
              />
              {user?.profilePicture && (
                <Button
                  size="sm"
                  isLoading={deleteProfilePictureMutation.isLoading}
                  onClick={async () =>
                    await deleteProfilePictureMutation.mutateAsync({ id: user?.id })
                  }
                >
                  <TrashIcon />
                </Button>
              )}
              {files && files.length > 0 && files[0].length > 0 && (
                <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                  <dt className="text-sm font-medium text-gray-500 dark:text-white">Preview:</dt>
                  <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">
                    <img
                      className="w-52 h-52 rounded-circle"
                      src={URL.createObjectURL(files[0][0])}
                      alt={`Preview of ${files[0][0].name}`}
                    />
                  </dd>
                </div>
              )}
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
