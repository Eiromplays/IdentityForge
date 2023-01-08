import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useClient } from '../api/getClient';
import { UpdateClientDTO, useUpdateClient } from '../api/updateClient';

const schema = z.object({
  clientId: z.string().min(1, 'Required'),
  clientName: z.string().min(1, 'Required'),
  description: z.string(),
  clientUri: z.string().url('Invalid URL').optional(),
  logoUri: z.string().url('Invalid URL').optional().nullable(),
  enabled: z.boolean(),
  requireConsent: z.boolean(),
  allowRememberConsent: z.boolean(),
});

type UpdateClientProps = {
  clientId: number;
};

export const UpdateClient = ({ clientId }: UpdateClientProps) => {
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
            await updateClientMutation.mutateAsync({ clientId: clientId, data: values });
          }}
          options={{
            defaultValues: {
              clientId: clientQuery.data?.clientId,
              clientName: clientQuery.data?.clientName,
              description: clientQuery.data?.description,
              clientUri: clientQuery.data?.clientUri,
              logoUri: clientQuery.data?.logoUri,
              enabled: clientQuery.data?.enabled,
              requireConsent: clientQuery.data?.requireConsent,
              allowRememberConsent: clientQuery.data?.allowRememberConsent,
            },
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="ClientId"
                error={{
                  errors: formState.errors,
                }}
                registration={register('clientId')}
              />
              <InputField
                label="ClientName"
                error={{
                  errors: formState.errors,
                }}
                registration={register('clientName')}
              />
              <InputField
                label="Description"
                error={{
                  errors: formState.errors,
                }}
                registration={register('description')}
              />
              <InputField
                label="Client Uri"
                error={{
                  errors: formState.errors,
                }}
                registration={register('clientUri')}
              />
              <InputField
                label="Logo Uri"
                error={{
                  errors: formState.errors,
                }}
                registration={register('logoUri')}
              />
              <InputField
                label="Enabled"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('enabled')}
              />
              <InputField
                label="Allow remember Consent"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('allowRememberConsent')}
              />
              <InputField
                label="Require Consent"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('requireConsent')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
