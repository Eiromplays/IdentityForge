import {
  Button,
  ConfirmationDialog,
  Spinner,
  Form,
  FormDrawer,
  InputField,
  ImageCropper,
} from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useUser } from '../api/getUser';
import { UpdateUserDTO, useUpdateUser } from '../api/updateUser';

const schema = z.object({
  username: z.string().min(1, 'Required'),
  displayName: z.string().min(1, 'Required'),
  firstName: z.string().min(1, 'Required'),
  lastName: z.string().min(1, 'Required'),
  email: z.string().min(1, 'Required'),
  gravatarEmail: z.string().nullable(),
  deleteCurrentImage: z.boolean(),
  revokeUserSessions: z.boolean(),
  emailConfirmed: z.boolean(),
  phoneNumberConfirmed: z.boolean(),
  isActive: z.boolean(),
  twoFactorEnabled: z.boolean(),
  lockoutEnabled: z.boolean(),
});

type UpdateUserProps = {
  id: string;
};

export const UpdateUser = ({ id }: UpdateUserProps) => {
  const userQuery = useUser({ userId: id || '' });
  const { updateUserMutation } = useUpdateUser();

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
        isDone={updateUserMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update User
          </Button>
        }
        title={`Update ${userQuery.data?.userName}'s Profile`}
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update User"
            body="Are you sure you want to update this user? This will log them out of all sessions."
            triggerButton={
              <Button size="sm" isLoading={updateUserMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-user"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateUserMutation.isLoading}
              >
                Update User
              </Button>
            }
          />
        }
      >
        <Form<UpdateUserDTO['data'], typeof schema>
          id="update-user"
          onSubmit={async (values) => {
            values.image = profilePicture;
            await updateUserMutation.mutateAsync({ userId: id, data: values });
          }}
          options={{
            defaultValues: {
              displayName: userQuery.data?.displayName,
              username: userQuery.data?.userName,
              firstName: userQuery.data?.firstName,
              lastName: userQuery.data?.lastName,
              email: userQuery.data?.email,
              gravatarEmail: userQuery.data?.gravatarEmail,
              deleteCurrentImage: false,
              revokeUserSessions: true,
              emailConfirmed: userQuery.data?.emailConfirmed,
              phoneNumberConfirmed: userQuery.data?.phoneNumberConfirmed,
              twoFactorEnabled: userQuery.data?.twoFactorEnabled,
              lockoutEnabled: userQuery.data?.lockoutEnabled,
              isActive: userQuery.data?.isActive,
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
                label="Display Name"
                error={formState.errors['displayName']}
                registration={register('displayName')}
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
                label="Phone Number"
                error={formState.errors['phoneNumber']}
                registration={register('phoneNumber')}
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
              <InputField
                label="Revoke User Session(s)"
                type="checkbox"
                error={formState.errors['revokeUserSessions']}
                registration={register('revokeUserSessions')}
              />
              <InputField
                label="Email Confirmed"
                type="checkbox"
                error={formState.errors['emailConfirmed']}
                registration={register('emailConfirmed')}
              />
              <InputField
                label="Phone Number Confirmed"
                type="checkbox"
                error={formState.errors['phoneNumberConfirmed']}
                registration={register('phoneNumberConfirmed')}
              />
              <InputField
                label="Two Factor Enabled"
                type="checkbox"
                error={formState.errors['twoFactorEnabled']}
                registration={register('twoFactorEnabled')}
              />
              <InputField
                label="Lockout Enabled"
                type="checkbox"
                error={formState.errors['lockoutEnabled']}
                registration={register('lockoutEnabled')}
              />
              <InputField
                label="Is Active"
                type="checkbox"
                error={formState.errors['isActive']}
                registration={register('isActive')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
