import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useClient } from '../api/getClient';
import { UpdateClientDTO, useUpdateClient } from '../api/updateClient';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  description: z.string().min(1, 'Required'),
});

type UpdateRoleProps = {
  clientId: string;
};

export const UpdateClient = ({ clientId }: UpdateRoleProps) => {
  const clientQuery = useClient({ clientId: clientId });
  const updateClientMutation = useUpdateClient();

  if (clientQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!clientQuery.data) return null;

  return (
    <>
      <FormDrawer
        isDone={updateClientMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update Client
          </Button>
        }
        title="Update Client"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update Client"
            body="Are you sure you want to update this client?"
            triggerButton={
              <Button size="sm" isLoading={updateClientMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-client"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateClientMutation.isLoading}
              >
                Update Client
              </Button>
            }
          />
        }
      >
        <Form<UpdateClientDTO['data'], typeof schema>
          id="update-client"
          onSubmit={async (values) => {
            values.id = clientId;
            await updateClientMutation.mutateAsync({ data: values });
          }}
          options={{
            defaultValues: {
              name: clientQuery.data?.clientName,
              description: clientQuery.data?.description,
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
