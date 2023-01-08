import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import {
  CreateIdentityResourceDTO,
  useCreateIdentityResource,
} from '../api/createIdentityResource';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  displayName: z.string().min(1, 'Required'),
  description: z.string(),
  showInDiscoveryDocument: z.boolean(),
  emphasize: z.boolean(),
  enabled: z.boolean(),
  required: z.boolean(),
  nonEditable: z.boolean(),
});

export const CreateIdentityResource = () => {
  const createIdentityResourceMutation = useCreateIdentityResource();

  return (
    <>
      <FormDrawer
        isDone={createIdentityResourceMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create IdentityResource
          </Button>
        }
        title="Create IdentityResource"
        submitButton={
          <Button
            form="create-identity-resource"
            type="submit"
            size="sm"
            isLoading={createIdentityResourceMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateIdentityResourceDTO['data'], typeof schema>
          id="create-identity-resource"
          onSubmit={async (values) => {
            await createIdentityResourceMutation.mutateAsync({
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
                label="Emphasize"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('emphasize')}
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
                label="Required"
                type="checkbox"
                error={{
                  errors: formState.errors,
                }}
                registration={register('required')}
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
