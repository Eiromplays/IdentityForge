import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateRoleClaimDTO, useCreateRoleClaim } from '../api/createRoleClaim';

const schema = z.object({
  type: z.string().min(1, 'Required'),
  value: z.string().min(1, 'Required'),
});

export type CreateUserClaimProps = {
  roleId: string;
};

export const CreateRoleClaim = ({ roleId }: CreateUserClaimProps) => {
  const createRoleClaimMutation = useCreateRoleClaim();

  return (
    <>
      <FormDrawer
        isDone={createRoleClaimMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create role claim
          </Button>
        }
        title="Create Role Claim"
        submitButton={
          <Button
            form="create-role-claim"
            type="submit"
            size="sm"
            isLoading={createRoleClaimMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateRoleClaimDTO['addRoleClaimRequest'], typeof schema>
          id="create-role-claim"
          onSubmit={async (values) => {
            await createRoleClaimMutation.mutateAsync({
              roleId: roleId,
              addRoleClaimRequest: values,
            });
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
