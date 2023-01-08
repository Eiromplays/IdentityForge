import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateApiResourceDTO, useCreateApiResource } from '../api/createApiResource';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  displayName: z.string().min(1, 'Required'),
  description: z.string(),
  showInDiscoveryDocument: z.boolean(),
  allowedAccessTokenSigningAlgorithms: z.string().nullable(),
  enabled: z.boolean(),
  requireResourceIndicator: z.boolean(),
  nonEditable: z.boolean(),
});

export const CreateApiResource = () => {
  const createApiResourceMutation = useCreateApiResource();

  return (
    <>
      <FormDrawer
        isDone={createApiResourceMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create ApiResource
          </Button>
        }
        title="Create ApiResource"
        submitButton={
          <Button
            form="create-api-resource"
            type="submit"
            size="sm"
            isLoading={createApiResourceMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateApiResourceDTO['data'], typeof schema>
          id="create-api-resource"
          onSubmit={async (values) => {
            await createApiResourceMutation.mutateAsync({
              data: values,
            });
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="Name"
                error={{
                  errors: formState.errors,
                }}
                registration={register('name')}
              />
              <InputField
                label="DisplayName"
                error={{
                  errors: formState.errors,
                }}
                registration={register('displayName')}
              />
              <InputField
                label="Description"
                error={{
                  errors: formState.errors,
                }}
                registration={register('description')}
              />
              <InputField
                label="Show In Discovery Document"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('showInDiscoveryDocument')}
              />
              <InputField
                label="allowedAccessTokenSigningAlgorithms"
                error={{
                  errors: formState.errors,
                }}
                registration={register('allowedAccessTokenSigningAlgorithms')}
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
                label="RequireResourceIndicator"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('requireResourceIndicator')}
              />
              <InputField
                label="NonEditable"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('nonEditable')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
