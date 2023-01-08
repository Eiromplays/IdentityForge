import { Button, ConfirmationDialog, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { UserRole } from '@/features/users';

import { UpdateUserRolesDTO, useUpdateUserRoles } from '../api/updateUserRoles';

const userRoleSchema = z.object({
  roleId: z.string().min(1, 'RoleId must be provided'),
  roleName: z.string().min(1, 'Role Name must be provided'),
  description: z.string().optional(),
  enabled: z.boolean(),
});

const schema = z.object({
  userRoles: z.array(userRoleSchema),
  revokeUserSessions: z.boolean(),
});

export type UpdateUserRolesProps = {
  id: string;
  roles: UserRole[];
};

export const UpdateUserRoles = ({ id, roles }: UpdateUserRolesProps) => {
  const updateUserRolesMutation = useUpdateUserRoles();

  if (!roles) return null;

  return (
    <div className="mt-4">
      <FormDrawer
        isDone={updateUserRolesMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update User Roles
          </Button>
        }
        title={`Update Roles`}
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update User Roles"
            body="Are you sure you want to update this users roles? This will log them out of all sessions."
            triggerButton={
              <Button size="sm" isLoading={updateUserRolesMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-user-roles"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateUserRolesMutation.isLoading}
              >
                Update User Roles
              </Button>
            }
          />
        }
      >
        <Form<UpdateUserRolesDTO['data'], typeof schema>
          id="update-user-roles"
          onSubmit={async (values) => {
            await updateUserRolesMutation.mutateAsync({ userId: id, data: values });
          }}
          options={{
            defaultValues: {
              userRoles: roles,
              revokeUserSessions: true,
            },
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              {roles?.map((role, index) => (
                <InputField
                  key={role.roleId}
                  label={role.roleName}
                  type="checkbox"
                  registration={register(`userRoles.${index}.enabled`)}
                  error={{
                    errors: formState.errors,
                  }}
                />
              ))}
              <div className="mt-7">
                <InputField
                  label="Revoke User Session(s)"
                  type="checkbox"
                  error={{
                    errors: formState.errors,
                  }}
                  registration={register('revokeUserSessions')}
                />
              </div>
            </>
          )}
        </Form>
      </FormDrawer>
    </div>
  );
};
