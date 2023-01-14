import {
  Button,
  ConfirmationDialog,
  Spinner,
  Form,
  FormDrawer,
  InputField,
  CustomInputField,
  ImageCropper,
} from 'eiromplays-ui';
import React from 'react';
import { HiOutlinePencil } from 'react-icons/hi';
import { isPossiblePhoneNumber } from 'react-phone-number-input';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

import { useUser } from '../api/getUser';
import { UpdateUserDTO, useUpdateUser } from '../api/updateUser';

const schema = z.object({
  username: z.string().min(1, 'Required'),
  displayName: z.string().min(1, 'Required'),
  firstName: z.string().min(1, 'Required'),
  lastName: z.string().min(1, 'Required'),
  phoneNumber: z
    .string()
    .nullable()
    .refine((v) => (v ? isPossiblePhoneNumber(v) : true), 'Invalid phone number'),
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
  const updateUserMutation = useUpdateUser();

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
              phoneNumber: userQuery.data?.phoneNumber,
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
          {({ register, formState, control }) => (
            <>
              <InputField
                label="Username"
                error={{
                  errors: formState.errors,
                }}
                registration={register('username')}
              />
              <InputField
                label="Display Name"
                error={{
                  errors: formState.errors,
                }}
                registration={register('displayName')}
              />
              <InputField
                label="First Name"
                error={{
                  errors: formState.errors,
                }}
                registration={register('firstName')}
              />
              <InputField
                label="Last Name"
                error={{
                  errors: formState.errors,
                }}
                registration={register('lastName')}
              />
              <CustomInputField
                label="Phone Number"
                error={{
                  name: 'phoneNumber',
                  errors: formState.errors,
                }}
                customInputField={
                  <PhoneInputWithCountry
                    className="bg-white dark:bg-gray-900 block px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                  'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                  'dark:focus:border-indigo-900 sm:text-sm"
                    name="phoneNumber"
                    control={control}
                    register={register('phoneNumber')}
                  />
                }
              />
              <InputField
                label="Email Address"
                type="email"
                error={{
                  errors: formState.errors,
                }}
                registration={register('email')}
              />
              <InputField
                label="Gravatar Email Address"
                type="email"
                error={{
                  errors: formState.errors,
                }}
                registration={register('gravatarEmail')}
              />
              <InputField
                label="Profile Picture"
                type="file"
                accept={'image/*'}
                error={{
                  errors: formState.errors,
                }}
                registration={register('image')}
              />
              {userQuery.data?.profilePicture && (
                <InputField
                  label="Delete profile picture"
                  type="checkbox"
                  error={{
                    errors: formState.errors,
                  }}
                  registration={register('deleteCurrentImage')}
                />
              )}
              {files && files.length > 0 && files[0].length > 0 && (
                <>
                  <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                    <ImageCropper
                      cropLabel="Crop:"
                      previewLabel="Preview:"
                      image={URL.createObjectURL(files[0][0])}
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
                error={{
                  errors: formState.errors,
                }}
                registration={register('revokeUserSessions')}
              />
              <InputField
                label="Email Confirmed"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('emailConfirmed')}
              />
              <InputField
                label="Phone Number Confirmed"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('phoneNumberConfirmed')}
              />
              <InputField
                label="Two Factor Enabled"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('twoFactorEnabled')}
              />
              <InputField
                label="Lockout Enabled"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('lockoutEnabled')}
              />
              <InputField
                label="Is Active"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('isActive')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
