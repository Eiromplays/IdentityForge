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
                error={formState.errors['name']}
                registration={register('name')}
              />
              <InputField
                label="DisplayName"
                error={formState.errors['displayName']}
                registration={register('displayName')}
              />
              <InputField
                label="Description"
                error={formState.errors['description']}
                registration={register('description')}
              />
              <InputField
                label="Show In Discovery Document"
                type="checkbox"
                error={formState.errors['showInDiscoveryDocument']}
                registration={register('showInDiscoveryDocument')}
              />
              <InputField
                label="Emphasize"
                type="checkbox"
                error={formState.errors['emphasize']}
                registration={register('emphasize')}
              />
              <InputField
                label="Enabled"
                type="checkbox"
                error={formState.errors['enabled']}
                registration={register('enabled')}
              />
              <InputField
                label="Required"
                type="checkbox"
                error={formState.errors['required']}
                registration={register('required')}
              />
              <InputField
                label="NonEditable"
                type="checkbox"
                error={formState.errors['nonEditable']}
                registration={register('nonEditable')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
