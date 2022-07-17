import { Button, ConfirmationDialog, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { UserClaim } from '@/features/users';

import { UpdateUserClaimDTO, useUpdateUserClaim } from '../api/updateUserClaim';

const schema = z.object({
  newType: z.string().min(1, 'Required'),
  newValue: z.string().min(1, 'Required'),
  valueType: z.string().nullable(),
  issuer: z.string().nullable(),
});

export type UpdateUserClaimProps = {
  id: string;
  userClaim: UserClaim;
};

export const UpdateUserClaim = ({ id, userClaim }: UpdateUserClaimProps) => {
  const updateUserClaimMutation = useUpdateUserClaim();

  return (
    <>
      <FormDrawer
        isDone={updateUserClaimMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update UserClaim
          </Button>
        }
        title={`Update UserClaim`}
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update UserClaim"
            body="Are you sure you want to update this claim?"
            triggerButton={
              <Button size="sm" isLoading={updateUserClaimMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-user-claim"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateUserClaimMutation.isLoading}
              >
                Update UserClaim
              </Button>
            }
          />
        }
      >
        <Form<UpdateUserClaimDTO['data'], typeof schema>
          id="update-user-claim"
          onSubmit={async (values) => {
            values.oldType = userClaim.type;
            values.oldValue = userClaim.value;
            await updateUserClaimMutation.mutateAsync({ userId: id, data: values });
          }}
          options={{
            defaultValues: {
              newType: userClaim.type,
              newValue: userClaim.value,
              valueType: userClaim.valueType,
              issuer: userClaim.issuer,
            },
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="Type"
                error={formState.errors['newType']}
                registration={register('newType')}
              />
              <InputField
                label="Value"
                error={formState.errors['newValue']}
                registration={register('newValue')}
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
