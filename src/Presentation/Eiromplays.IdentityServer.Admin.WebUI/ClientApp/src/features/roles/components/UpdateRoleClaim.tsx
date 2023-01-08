import { Button, ConfirmationDialog, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { RoleClaim } from '@/features/roles';

import { UpdateRoleClaimDTO, useUpdateRoleClaim } from '../api/updateUserClaim';

const schema = z.object({
  type: z.string().min(1, 'Required'),
  value: z.string().min(1, 'Required'),
});

export type UpdateRoleClaimProps = {
  roleId: string;
  roleClaim: RoleClaim;
};

export const UpdateRoleClaim = ({ roleId, roleClaim }: UpdateRoleClaimProps) => {
  const updateRoleClaimMutation = useUpdateRoleClaim();

  return (
    <>
      <FormDrawer
        isDone={updateRoleClaimMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update Role Claim
          </Button>
        }
        title={`Update Role Claim`}
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update Role Claim"
            body="Are you sure you want to update this claim?"
            triggerButton={
              <Button size="sm" isLoading={updateRoleClaimMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-role-claim"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateRoleClaimMutation.isLoading}
              >
                Update Role Claim
              </Button>
            }
          />
        }
      >
        <Form<UpdateRoleClaimDTO['data'], typeof schema>
          id="update-role-claim"
          onSubmit={async (values) => {
            await updateRoleClaimMutation.mutateAsync({
              roleId: roleId,
              claimId: roleClaim.id,
              data: values,
            });
          }}
          options={{
            defaultValues: {
              type: roleClaim.claim.type,
              value: roleClaim.claim.value,
            },
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="Type"
                error={{
                  errors: formState.errors,
                }}
                registration={register('type')}
              />
              <InputField
                label="Value"
                error={{
                  errors: formState.errors,
                }}
                registration={register('value')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
