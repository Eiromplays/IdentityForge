import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useIdentityResource } from '../api/getIdentityResource';
import {
  UpdateIdentityResourceDTO,
  useUpdateIdentityResource,
} from '../api/updateIdentityResource';

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

type UpdateIdentityResourceProps = {
  identityResourceId: number;
};

export const UpdateIdentityResource = ({ identityResourceId }: UpdateIdentityResourceProps) => {
  const identityResourceQuery = useIdentityResource({ identityResourceId: identityResourceId });
  const updateIdentityResourceMutation = useUpdateIdentityResource();

  if (identityResourceQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!identityResourceQuery.data) return null;

  return (
    <>
      <FormDrawer
        isDone={updateIdentityResourceMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update IdentityResource
          </Button>
        }
        title="Update IdentityResource"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update IdentityResource"
            body="Are you sure you want to update this IdentityResource?"
            triggerButton={
              <Button size="sm" isLoading={updateIdentityResourceMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-identity-resource"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateIdentityResourceMutation.isLoading}
              >
                Update IdentityResource
              </Button>
            }
          />
        }
      >
        <Form<UpdateIdentityResourceDTO['data'], typeof schema>
          id="update-identity-resource"
          onSubmit={async (values) => {
            values.id = identityResourceId;
            await updateIdentityResourceMutation.mutateAsync({
              identityResourceId: identityResourceId,
              data: values,
            });
          }}
          options={{
            defaultValues: {
              name: identityResourceQuery.data?.name,
              displayName: identityResourceQuery.data?.displayName,
              description: identityResourceQuery.data?.description,
              showInDiscoveryDocument: identityResourceQuery.data?.showInDiscoveryDocument,
              emphasize: identityResourceQuery.data?.emphasize,
              enabled: identityResourceQuery.data?.enabled,
              required: identityResourceQuery.data?.required,
              nonEditable: identityResourceQuery.data?.nonEditable,
            },
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
