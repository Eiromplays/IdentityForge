import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil, HiOutlineTrash } from 'react-icons/hi';
import * as z from 'zod';

import {
  CreateIdentityProviderDTO,
  useCreateIdentityProvider,
} from '../api/createIdentityProvider';

const schema = z.object({
  scheme: z.string().min(1, 'Required'),
  displayName: z.string().min(1, 'Required'),
  enabled: z.boolean(),
  type: z.string().default('oidc'),
  properties: z.record(z.string(), z.string()),
});

export const CreateIdentityProvider = () => {
  const createIdentityProviderMutation = useCreateIdentityProvider();

  return (
    <>
      <FormDrawer
        isDone={createIdentityProviderMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create IdentityProvider
          </Button>
        }
        title="Create IdentityProvider"
        submitButton={
          <Button
            form="create-identity-provider"
            type="submit"
            size="sm"
            isLoading={createIdentityProviderMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateIdentityProviderDTO['data'] & { addProperty: string }, typeof schema>
          id="create-identity-provider"
          onSubmit={async (values) => {
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            await createIdentityProviderMutation.mutateAsync({
              data: values,
            });
          }}
          schema={schema}
          options={{
            defaultValues: {
              enabled: true,
              type: 'oidc',
              properties: {
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
