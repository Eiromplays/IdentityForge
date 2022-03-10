import { PencilIcon, TrashIcon } from '@heroicons/react/solid';
import * as z from 'zod';

import { Button, ConfirmationDialog } from '@/components/Elements';
import { Form, FormDrawer, InputField } from '@/components/Form';
import { ImageCropper } from '@/components/Images';
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
  let profilePicture: File;
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
          <ConfirmationDialog
            icon="warning"
            title="Update Profile"
            body="Are you sure you want to update your profile? This will require you to log back in."
            triggerButton={
              <Button size="sm" isLoading={updateProfileMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-profile"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateProfileMutation.isLoading}
              >
                Update Profile
              </Button>
            }
          />
        }
      >
        <Form<UpdateProfileDTO['data'], typeof schema>
          id="update-profile"
          onSubmit={async (values) => {
            values.profilePicture = profilePicture;
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
                <ConfirmationDialog
                  icon="danger"
                  title="Delete Profile Picture"
                  body="Are you sure you want to delete your profile Picture?"
                  triggerButton={
                    <Button
                      className="mt-2"
                      variant="inverse"
                      size="sm"
                      isLoading={deleteProfilePictureMutation.isLoading}
                    >
                      <TrashIcon className="h-4 w-4 text-red-700" />
                    </Button>
                  }
                  confirmButton={
                    <Button
                      className="mt-2"
                      variant="danger"
                      size="sm"
                      isLoading={deleteProfilePictureMutation.isLoading}
                      onClick={async () =>
                        await deleteProfilePictureMutation.mutateAsync({ id: user?.id })
                      }
                    >
                      Delete
                    </Button>
                  }
                />
              )}
              {files && files.length > 0 && files[0].length > 0 && (
                <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                  <dt className="text-sm font-medium text-gray-500 dark:text-white">Preview:</dt>
                  <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">
                    <ImageCropper
                      imgSrc={URL.createObjectURL(files[0][0])}
                      fileName={files[0][0].name}
                      onFileCreated={(file: File) => {
                        profilePicture = file;
                      }}
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
