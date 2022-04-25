import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { Button, ConfirmationDialog, Spinner } from '@/components/Elements';
import { Form, FormDrawer, InputField } from '@/components/Form';

import { useRole } from '../api/getRole';
import { UpdateRoleDTO, useUpdateRole } from '../api/updateRole';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  description: z.string().min(1, 'Required'),
});

type UpdateRoleProps = {
  id: string;
};

export const UpdateRole = ({ id }: UpdateRoleProps) => {
  const roleQuery = useRole({ roleId: id || '' });
  const updateRoleMutation = useUpdateRole();

  if (roleQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!roleQuery.data) return null;

  return (
    <>
      <FormDrawer
        isDone={updateRoleMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update Role
          </Button>
        }
        title="Update Role"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update Role"
            body="Are you sure you want to update this role?"
            triggerButton={
              <Button size="sm" isLoading={updateRoleMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-role"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateRoleMutation.isLoading}
              >
                Update Role
              </Button>
            }
          />
        }
      >
        <Form<UpdateRoleDTO['data'], typeof schema>
          id="update-role"
          onSubmit={async (values) => {
            values.id = id;
            await updateRoleMutation.mutateAsync({ data: values });
          }}
          options={{
            defaultValues: {
              name: roleQuery.data?.name,
              description: roleQuery.data?.description,
            },
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="Name"
                error={formState.errors['name']}
                registration={register('name')}
              />
              <InputField
                label="Description"
                error={formState.errors['description']}
                registration={register('description')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
