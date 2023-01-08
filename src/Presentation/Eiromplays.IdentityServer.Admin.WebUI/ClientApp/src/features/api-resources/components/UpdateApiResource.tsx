import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useApiResource } from '../api/getApiResource';
import { UpdateApiResourceDTO, useUpdateApiResource } from '../api/updateApiResource';

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

type UpdateIdentityResourceProps = {
  apiResourceId: number;
};

export const UpdateApiResource = ({ apiResourceId }: UpdateIdentityResourceProps) => {
  const apiResourceQuery = useApiResource({ apiResourceId: apiResourceId });
  const updateApiResourceMutation = useUpdateApiResource();

  if (apiResourceQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!apiResourceQuery.data) return null;

  return (
    <>
      <FormDrawer
        isDone={updateApiResourceMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update ApiResource
          </Button>
        }
        title="Update ApiResource"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update ApiResource"
            body="Are you sure you want to update this ApiResource?"
            triggerButton={
              <Button size="sm" isLoading={updateApiResourceMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-api-resource"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateApiResourceMutation.isLoading}
              >
                Update ApiResource
              </Button>
            }
          />
        }
      >
        <Form<UpdateApiResourceDTO['data'], typeof schema>
          id="update-api-resource"
          onSubmit={async (values) => {
            values.id = apiResourceId;
            await updateApiResourceMutation.mutateAsync({
              apiResourceId: apiResourceId,
              data: values,
            });
          }}
          options={{
            defaultValues: {
              name: apiResourceQuery.data?.name,
              displayName: apiResourceQuery.data?.displayName,
              description: apiResourceQuery.data?.description,
              showInDiscoveryDocument: apiResourceQuery.data?.showInDiscoveryDocument,
              allowedAccessTokenSigningAlgorithms:
                apiResourceQuery.data?.allowedAccessTokenSigningAlgorithms,
              enabled: apiResourceQuery.data?.enabled,
              requireResourceIndicator: apiResourceQuery.data?.requireResourceIndicator,
              nonEditable: apiResourceQuery.data?.nonEditable,
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
