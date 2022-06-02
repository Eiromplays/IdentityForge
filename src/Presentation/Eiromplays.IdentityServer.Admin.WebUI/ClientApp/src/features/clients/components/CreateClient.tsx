import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateClientDTO, useCreateClient } from '../api/createClient';

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

export const CreateClient = () => {
  const createClientMutation = useCreateClient();

  return (
    <>
      <FormDrawer
        isDone={createClientMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create Client
          </Button>
        }
        title="Create Client"
        submitButton={
          <Button
            form="create-client"
            type="submit"
            size="sm"
            isLoading={createClientMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateClientDTO['data'], typeof schema>
          id="create-client"
          onSubmit={async (values) => {
            await createClientMutation.mutateAsync({ data: values });
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="ClientId"
                error={formState.errors['clientId']}
                registration={register('clientId')}
              />
              <InputField
                label="ClientName"
                error={formState.errors['clientName']}
                registration={register('clientName')}
              />
              <InputField
                label="Description"
                error={formState.errors['description']}
                registration={register('description')}
              />
              <InputField
                label="Client Uri"
                error={formState.errors['clientUri']}
                registration={register('clientUri')}
              />
              <InputField
                label="Logo Uri"
                error={formState.errors['logoUri']}
                registration={register('logoUri')}
              />
              <InputField
                label="Enabled"
                type="checkbox"
                error={formState.errors['enabled']}
                registration={register('enabled')}
              />
              <InputField
                label="Allow remember Consent"
                type="checkbox"
                error={formState.errors['allowRememberConsent']}
                registration={register('allowRememberConsent')}
              />
              <InputField
                label="Require Consent"
                type="checkbox"
                error={formState.errors['requireConsent']}
                registration={register('requireConsent')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
