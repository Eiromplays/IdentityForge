import { Button, ConfirmationDialog, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { UserClaim } from '@/features/users';

import { UpdateUserClaimDTO, useUpdateUserClaim } from '../api/updateUserClaim';

const schema = z.object({
  type: z.string().min(1, 'Required'),
  value: z.string().min(1, 'Required'),
});

export type UpdateUserClaimProps = {
  userId: string;
  userClaim: UserClaim;
};

export const UpdateUserClaim = ({ userId, userClaim }: UpdateUserClaimProps) => {
  const updateUserClaimMutation = useUpdateUserClaim();

  return (
    <>
      <FormDrawer
        isDone={updateUserClaimMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update User Claim
          </Button>
        }
        title={`Update User Claim`}
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update User Claim"
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
                Update User Claim
              </Button>
            }
          />
        }
      >
        <Form<UpdateUserClaimDTO['data'], typeof schema>
          id="update-user-claim"
          onSubmit={async (values) => {
            await updateUserClaimMutation.mutateAsync({
              userId: userId,
              claimId: userClaim.id,
              data: values,
            });
          }}
          options={{
            defaultValues: {
              type: userClaim.claim.type,
              value: userClaim.claim.value,
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
