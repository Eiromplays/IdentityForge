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
  id: string;
};

export const UpdateClient = ({ id }: UpdateRoleProps) => {
  const clientQuery = useClient({ clientId: id || '' });
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
              <Button size="sm" isLoading={updateClientMutation.isLoading}>
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
                isLoading={updateClientMutation.isLoading}
              >
                Update Role
              </Button>
            }
          />
        }
      >
        <Form<UpdateClientDTO['data'], typeof schema>
          id="update-role"
          onSubmit={async (values) => {
            values.id = id;
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
