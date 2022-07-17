import { Button, CustomInputField, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

import { CreateUserClaimDTO, useCreateUserClaim } from '../api/createUseClaim';

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
            Create UserClaim
          </Button>
        }
        title="Create UserClaim"
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
                error={formState.errors['type']}
                registration={register('type')}
              />
              <InputField
                label="Value"
                error={formState.errors['value']}
                registration={register('value')}
              />
              <InputField
                label="ValueType"
                error={formState.errors['valueType']}
                registration={register('valueType')}
              />
              <InputField
                label="Issuer"
                error={formState.errors['issuer']}
                registration={register('issuer')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
