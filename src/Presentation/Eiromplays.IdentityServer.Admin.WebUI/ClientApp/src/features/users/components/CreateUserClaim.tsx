import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateUserClaimDTO, useCreateUserClaim } from '../api/createUserClaim';

const schema = z.object({
  type: z.string().min(1, 'Required'),
  value: z.string().min(1, 'Required'),
  valueType: z.string().nullable(),
  issuer: z.string().nullable(),
});

export type CreateUserClaimProps = {
  id: string;
};

export const CreateUserClaim = ({ id }: CreateUserClaimProps) => {
  const createUserClaimMutation = useCreateUserClaim();

  return (
    <>
      <FormDrawer
        isDone={createUserClaimMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create User Claim
          </Button>
        }
        title="Create User Claim"
        submitButton={
          <Button
            form="create-user-claim"
            type="submit"
            size="sm"
            isLoading={createUserClaimMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateUserClaimDTO['addUserClaimRequest'], typeof schema>
          id="create-user-claim"
          onSubmit={async (values) => {
            await createUserClaimMutation.mutateAsync({ userId: id, addUserClaimRequest: values });
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
              <InputField
                label="ValueType"
                error={{
                  errors: formState.errors,
                }}
                registration={register('valueType')}
              />
              <InputField
                label="Issuer"
                error={{
                  errors: formState.errors,
                }}
                registration={register('issuer')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
