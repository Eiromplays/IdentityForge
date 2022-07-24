import { Button, Form, FormDrawer, InputField, useTheme } from 'eiromplays-ui';
import { Controller } from 'react-hook-form';
import { HiOutlinePencil } from 'react-icons/hi';
import makeAnimated from 'react-select/animated';
import CreatableSelect from 'react-select/creatable';
import * as z from 'zod';

import { CreateClientDTO, useCreateClient } from '../api/createClient';

const schema = z.object({
  enabled: z.boolean(),
  clientId: z.string().min(1, 'Required'),
  protocolType: z.string().min(1, 'Required'),
  requireClientSecret: z.boolean(),
  clientName: z.string().min(1, 'Required'),
  description: z.string(),
  clientUri: z.string().url('Invalid URL').optional(),
  logoUri: z.string().url('Invalid URL').optional().nullable(),
  requireConsent: z.boolean(),
  allowRememberConsent: z.boolean(),
  alwaysIncludeUserClaimsInIdToken: z.boolean(),
  allowedGrantTypes: z.array(z.string()).min(1, 'Required'),
  requirePkce: z.boolean(),
  allowPlainTextPkce: z.boolean(),
  requireRequestObject: z.boolean(),
  allowAccessTokensViaBrowser: z.boolean(),
});

const animatedComponents = makeAnimated();

export const CreateClient = () => {
  const createClientMutation = useCreateClient();
  const { currentTheme } = useTheme();

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
          options={{
            defaultValues: {
              enabled: true,
              protocolType: 'oidc',
              allowRememberConsent: true,
              allowedGrantTypes: ['authorization_code'],
              requirePkce: true,
            },
          }}
        >
          {({ register, formState, control, getValues }) => (
            <>
              <InputField
                label="Enabled"
                type="checkbox"
                error={formState.errors['enabled']}
                registration={register('enabled')}
              />
              <InputField
                label="ClientId"
                error={formState.errors['clientId']}
                registration={register('clientId')}
              />
              <InputField
                label="Protocol Type"
                error={formState.errors['protocolType']}
                registration={register('protocolType')}
              />
              <InputField
                label="Require Client Secret"
                type="checkbox"
                error={formState.errors['requireClientSecret']}
                registration={register('requireClientSecret')}
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
                label="Require Consent"
                type="checkbox"
                error={formState.errors['requireConsent']}
                registration={register('requireConsent')}
              />
              <InputField
                label="Allow remember Consent"
                type="checkbox"
                error={formState.errors['allowRememberConsent']}
                registration={register('allowRememberConsent')}
              />
              <InputField
                label="Always Include User Claims In Id Token"
                type="checkbox"
                error={formState.errors['alwaysIncludeUserClaimsInIdToken']}
                registration={register('alwaysIncludeUserClaimsInIdToken')}
              />
              <Controller
                control={control}
                defaultValue={getValues('allowedGrantTypes')}
                name="allowedGrantTypes"
                render={({ field: { onChange, ref } }) => (
                  <CreatableSelect
                    ref={ref}
                    defaultValue={
                      getValues('allowedGrantTypes').map((grantType) => ({
                        value: grantType,
                        label: grantType,
                      })) as any
                    }
                    isMulti
                    onChange={(val) => onChange(val.map((c: any) => c.value))}
                    options={
                      getValues('allowedGrantTypes').map((grantType) => ({
                        value: grantType,
                        label: grantType,
                      })) as any
                    }
                    components={animatedComponents as any}
                    theme={(theme) =>
                      currentTheme === 'dark'
                        ? {
                            ...theme,
                            colors: {
                              ...theme.colors,
                              primary: '#0a0e17',
                              primary25: 'gray',
                              primary50: '#fff',
                              neutral0: '#0a0e17',
                            },
                          }
                        : {
                            ...theme,
                            colors: {
                              ...theme.colors,
                            },
                          }
                    }
                  />
                )}
              />
              <InputField
                label="Require PKCE"
                type="checkbox"
                error={formState.errors['requirePkce']}
                registration={register('requirePkce')}
              />
              <InputField
                label="Allow Plain Text PKCE"
                type="checkbox"
                error={formState.errors['allowPlainTextPkce']}
                registration={register('allowPlainTextPkce')}
              />
              <InputField
                label="Require Request Object"
                type="checkbox"
                error={formState.errors['requireRequestObject']}
                registration={register('requireRequestObject')}
              />
              <InputField
                label="Allow Access Tokens Via Browser"
                type="checkbox"
                error={formState.errors['allowAccessTokensViaBrowser']}
                registration={register('allowAccessTokensViaBrowser')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
