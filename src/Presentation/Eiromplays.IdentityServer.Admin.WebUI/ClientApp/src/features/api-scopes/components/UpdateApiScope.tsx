import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useApiScope } from '../api/getApiScope';
import { UpdateApiScopeDTO, useUpdateApiScope } from '../api/updateApiScope';

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

type UpdateApiScopeProps = {
  apiScopeId: number;
};

export const UpdateApiScope = ({ apiScopeId }: UpdateApiScopeProps) => {
  const apiScopeQuery = useApiScope({ apiScopeId: apiScopeId });
  const updateApiScopeMutation = useUpdateApiScope();

  if (apiScopeQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!apiScopeQuery.data) return null;

  return (
    <>
      <FormDrawer
        isDone={updateApiScopeMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update ApiScope
          </Button>
        }
        title="Update ApiScope"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update ApiScope"
            body="Are you sure you want to update this ApiScope?"
            triggerButton={
              <Button size="sm" isLoading={updateApiScopeMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-api-scope"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateApiScopeMutation.isLoading}
              >
                Update ApiScope
              </Button>
            }
          />
        }
      >
        <Form<UpdateApiScopeDTO['data'], typeof schema>
          id="update-api-scope"
          onSubmit={async (values) => {
            values.id = apiScopeId;
            await updateApiScopeMutation.mutateAsync({
              apiScopeId: apiScopeId,
              data: values,
            });
          }}
          options={{
            defaultValues: {
              name: apiScopeQuery.data?.name,
              displayName: apiScopeQuery.data?.displayName,
              description: apiScopeQuery.data?.description,
              showInDiscoveryDocument: apiScopeQuery.data?.showInDiscoveryDocument,
              emphasize: apiScopeQuery.data?.emphasize,
              enabled: apiScopeQuery.data?.enabled,
              required: apiScopeQuery.data?.required,
              nonEditable: apiScopeQuery.data?.nonEditable,
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
