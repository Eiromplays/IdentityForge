import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil, HiOutlineTrash } from 'react-icons/hi';
import * as z from 'zod';

import { useIdentityProvider } from '../api/getIdentityProvider';
import {
  UpdateIdentityProviderDTO,
  useUpdateIdentityProvider,
} from '../api/updateIdentityProvider';

const schema = z.object({
  scheme: z.string().min(1, 'Required'),
  displayName: z.string().min(1, 'Required'),
  enabled: z.boolean(),
  type: z.string().default('oidc'),
  properties: z.record(z.string(), z.string()),
});

type UpdateIdentityProviderProps = {
  identityProviderId: number;
};

export const UpdateIdentityProvider = ({ identityProviderId }: UpdateIdentityProviderProps) => {
  const identityProviderQuery = useIdentityProvider({ identityProviderId: identityProviderId });
  const updateIdentityProviderMutation = useUpdateIdentityProvider();

  if (identityProviderQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!identityProviderQuery.data) return null;

  return (
    <>
      <FormDrawer
        isDone={updateIdentityProviderMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update IdentityProvider
          </Button>
        }
        title="Update IdentityProvider"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update IdentityProvider"
            body="Are you sure you want to update this IdentityProvider?"
            triggerButton={
              <Button size="sm" isLoading={updateIdentityProviderMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-identity-provider"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateIdentityProviderMutation.isLoading}
              >
                Update IdentityProvider
              </Button>
            }
          />
        }
      >
        <Form<UpdateIdentityProviderDTO['data'] & { addProperty: string }, typeof schema>
          id="update-identity-provider"
          onSubmit={async (values) => {
            values.id = identityProviderId;
            await updateIdentityProviderMutation.mutateAsync({
              identityProviderId: identityProviderId,
              data: values,
            });
          }}
          options={{
            defaultValues: {
              scheme: identityProviderQuery.data?.scheme,
              displayName: identityProviderQuery.data?.displayName,
              enabled: identityProviderQuery.data?.enabled ?? true,
              type: identityProviderQuery.data?.type ?? 'oidc',
              properties: identityProviderQuery.data?.properties ?? {
                Authority: '',
                ResponseType: 'id_token',
                ClientId: '',
                ClientSecret: '',
                Scope: 'openid',
                GetClaimsFromUserInfoEndpoint: 'true',
                UsePkce: 'true',
              },
            },
          }}
          schema={schema}
        >
          {({ register, formState, getValues, setValue }) => (
            <>
              <InputField
                label="Scheme"
                error={{
                  errors: formState.errors,
                }}
                registration={register('scheme')}
              />
              <InputField
                label="DisplayName"
                error={{
                  errors: formState.errors,
                }}
                registration={register('displayName')}
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
                label="Type"
                error={{
                  errors: formState.errors,
                }}
                registration={register('type')}
              />
              <p className="block text-sm font-medium text-gray-700 dark:text-white">Properties:</p>
              {Object.entries(getValues().properties).map(([key]) => (
                <div key={key} className="flex flex-wrap gap-2">
                  <InputField
                    label={key}
                    error={{
                      errors: formState.errors,
                    }}
                    registration={register(`properties.${key}`)}
                  />
                  <Button
                    variant="danger"
                    className="mt-6"
                    onClick={() => {
                      const properties = getValues('properties');
                      delete properties[key];
                      setValue('properties', properties);
                    }}
                  >
                    <HiOutlineTrash className="h-4 w-4" />
                  </Button>
                </div>
              ))}
              <div className="mt-2">
                <InputField
                  label="Add Property"
                  type="text"
                  error={{
                    errors: formState.errors,
                  }}
                  registration={register('addProperty')}
                />
                {getValues('addProperty') ? (
                  <Button
                    size="sm"
                    className="mt-2"
                    onClick={() => {
                      const { properties, addProperty } = getValues();

                      (properties ?? {})[addProperty] = '';

                      setValue('properties', properties);
                      setValue('addProperty', '');
                    }}
                  >
                    Add {getValues('addProperty')}
                  </Button>
                ) : null}
              </div>
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
