import {
  Button,
  ConfirmationDialog,
  Form,
  FormDrawer,
  InputField,
  ImageCropper,
  useAuth,
  CustomInputField,
} from 'eiromplays-ui';
import React from 'react';
import { HiOutlinePencil } from 'react-icons/hi';
import { isPossiblePhoneNumber } from 'react-phone-number-input';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

import { UpdateProfileDTO, useUpdateProfile } from '../api/updateProfile';

const schema = z.object({
  firstName: z.string().min(1, 'Required'),
  lastName: z.string().min(1, 'Required'),
  email: z.string().min(1, 'Required'),
  gravatarEmail: z.string(),
  deleteCurrentImage: z.boolean(),
  phoneNumber: z
    .string()
    .nullable()
    .refine((v) => (v ? isPossiblePhoneNumber(v) : true), 'Invalid phone number'),
});

export const UpdateProfile = () => {
  const { user } = useAuth();
  let files: FileList[] = [];
  let profilePicture: File;
  const { updateProfileMutation } = useUpdateProfile();

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
            values.image = profilePicture;
            values.id = user.id;
            await updateProfileMutation.mutateAsync({ data: values });
          }}
          options={{
            defaultValues: {
              username: user?.username,
              firstName: user?.firstName,
              lastName: user?.lastName,
              email: user?.email,
              gravatarEmail: user?.gravatarEmail,
              deleteCurrentImage: false,
              phoneNumber: user?.phone_number,
            },
          }}
          schema={schema}
          onChange={(_, file) => (files = file)}
        >
          {({ register, formState, control }) => (
            <>
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
              <CustomInputField
                label="Phone Number"
                error={formState.errors['phoneNumber']}
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
                label="Profile Picture"
                type="file"
                accept={'image/*'}
                error={formState.errors['image']}
                registration={register('image')}
              />
              {user?.profilePicture && (
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
