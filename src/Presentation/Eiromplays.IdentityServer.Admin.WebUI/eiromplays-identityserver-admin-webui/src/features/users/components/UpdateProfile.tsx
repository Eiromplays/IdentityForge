import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField, ImageCropper } from 'eiromplays-ui';

import { useUser } from '../api/getUser';
import { UpdateProfileDTO, useUpdateProfile } from '../api/updateProfile';

const schema = z.object({
  username: z.string().min(1, 'Required'),
  firstName: z.string().min(1, 'Required'),
  lastName: z.string().min(1, 'Required'),
  email: z.string().min(1, 'Required'),
  deleteCurrentImage: z.boolean(),
});

type UpdateProfileProps = {
  id: string;
};

export const UpdateProfile = ({ id }: UpdateProfileProps) => {
  const userQuery = useUser({ userId: id || '' });
  const { updateProfileMutation } = useUpdateProfile();

  if (userQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!userQuery.data) return null;

  let files: FileList[] = [];
  let profilePicture: File;

  return (
    <>
      <FormDrawer
        isDone={updateProfileMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update Profile
          </Button>
        }
        title="Update Profile"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update Profile"
            body="Are you sure you want to update this profile? This will log them out of all sessions."
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
            values.image = profilePicture;
            await updateProfileMutation.mutateAsync({ userId: id, data: values });
          }}
          options={{
            defaultValues: {
              username: userQuery.data?.userName,
              firstName: userQuery.data?.firstName,
              lastName: userQuery.data?.lastName,
              email: userQuery.data?.email,
              gravatarEmail: userQuery.data?.gravatarEmail,
              deleteCurrentImage: false,
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
                label="First Name"
                error={formState.errors['firstName']}
                registration={register('firstName')}
              />
              <InputField
                label="Last Name"
                error={formState.errors['lastName']}
                registration={register('lastName')}
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
                error={formState.errors['image']}
                registration={register('image')}
              />
              {userQuery.data?.profilePicture && (
                <InputField
                  label="Delete profile picture"
                  type="checkbox"
                  error={formState.errors['deleteCurrentImage']}
                  registration={register('deleteCurrentImage')}
                />
              )}
              {files && files.length > 0 && files[0].length > 0 && (
                <>
                  <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                    <ImageCropper
                      cropLabel="Crop:"
                      previewLabel="Preview:"
                      imgSrc={URL.createObjectURL(files[0][0])}
                      fileName={files[0][0].name}
                      onFileCreated={(file: File) => {
                        profilePicture = file;
                      }}
                    />
                  </div>
                </>
              )}
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
