import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useRole } from '../api/getRole';
import { UpdateRoleDTO, useUpdateRole } from '../api/updateRole';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  description: z.string().optional(),
});

export type UpdateRoleProps = {
  roleId: string;
};

export const UpdateRole = ({ roleId }: UpdateRoleProps) => {
  const roleQuery = useRole({ roleId: roleId || '' });
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
            values.id = roleId;
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
                error={{
                  errors: formState.errors,
                }}
                registration={register('name')}
              />
              <InputField
                label="Description"
                error={{
                  errors: formState.errors,
                }}
                registration={register('description')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
