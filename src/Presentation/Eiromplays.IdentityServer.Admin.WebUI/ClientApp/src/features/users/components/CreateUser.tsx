import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateUserDTO, useCreateUser } from '../api/createUser';

const schema = z
  .object({
    username: z.string().min(1, 'Required'),
    firstName: z.string().min(1, 'Required'),
    lastName: z.string().min(1, 'Required'),
    email: z.string().min(1, 'Required'),
    password: z.string().min(1, 'Required'),
    confirmPassword: z.string().min(1, 'Required'),
    phoneNumber: z.string(),
  })
  .refine((data) => data.confirmPassword === data.password, {
    message: 'Passwords do not match',
    path: ['confirmPassword'],
  });

export const CreateUser = () => {
  const createUserMutation = useCreateUser();

  return (
    <>
      <FormDrawer
        isDone={createUserMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create User
          </Button>
        }
        title="Create User"
        submitButton={
          <Button
            form="create-user"
            type="submit"
            size="sm"
            isLoading={createUserMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateUserDTO['data'], typeof schema>
          id="create-user"
          onSubmit={async (values) => {
            await createUserMutation.mutateAsync({ data: values });
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
                label="Email"
                type="email"
                error={formState.errors['email']}
                registration={register('email')}
              />
              <InputField
                label="Password"
                type="password"
                error={formState.errors['password']}
                registration={register('password')}
              />
              <InputField
                label="Confirm Password"
                type="password"
                error={formState.errors['confirmPassword']}
                registration={register('confirmPassword')}
              />
              <InputField
                label="Phone Number"
                type="tel"
                error={formState.errors['phoneNumber']}
                registration={register('phoneNumber')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
