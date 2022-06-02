import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateApiScopeDTO, useCreateApiScope } from '../api/createApiScope';

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

export const CreateApiScope = () => {
  const createApiScopeMutation = useCreateApiScope();

  return (
    <>
      <FormDrawer
        isDone={createApiScopeMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create ApiScope
          </Button>
        }
        title="Create ApiScope"
        submitButton={
          <Button
            form="create-api-scope"
            type="submit"
            size="sm"
            isLoading={createApiScopeMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateApiScopeDTO['data'], typeof schema>
          id="create-api-scope"
          onSubmit={async (values) => {
            await createApiScopeMutation.mutateAsync({
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
