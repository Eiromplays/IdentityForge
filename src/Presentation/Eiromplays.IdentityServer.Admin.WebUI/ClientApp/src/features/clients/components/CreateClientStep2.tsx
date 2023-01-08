import {
  CustomInputField,
  InputField,
  MultiStepForm,
  StepComponentProps,
  useStepper,
  useTheme,
} from 'eiromplays-ui';
import React from 'react';
import { Controller } from 'react-hook-form';
import makeAnimated from 'react-select/animated';
import CreatableSelect from 'react-select/creatable';
import * as z from 'zod';

import { CreateClientDTO } from '../api/createClient';

export type CreateClientStep2DTO = Pick<
  CreateClientDTO['data'],
  | 'alwaysIncludeUserClaimsInIdToken'
  | 'allowedGrantTypes'
  | 'requirePkce'
  | 'allowPlainTextPkce'
  | 'requireRequestObject'
  | 'allowAccessTokensViaBrowser'
  | 'redirectUris'
  | 'postLogoutRedirectUris'
  | 'frontChannelLogoutUri'
  | 'frontChannelLogoutSessionRequired'
>;

const schema = z.object({
  alwaysIncludeUserClaimsInIdToken: z.boolean(),
  allowedGrantTypes: z.array(z.string()).min(1, 'Required'),
  requirePkce: z.boolean(),
  allowPlainTextPkce: z.boolean(),
  requireRequestObject: z.boolean(),
  allowAccessTokensViaBrowser: z.boolean(),
  redirectUris: z.array(z.string().url('Invalid URL').optional().or(z.literal(''))).optional(),
  postLogoutRedirectUris: z
    .array(z.string().url('Invalid URL').optional().or(z.literal('')))
    .optional(),
  frontChannelLogoutUri: z.string().url('Invalid URL').optional().or(z.literal('')),
  frontChannelLogoutSessionRequired: z.boolean(),
});
const animatedComponents = makeAnimated();

export const CreateClientStep2 = ({ onFormCompleted }: StepComponentProps) => {
  const { formValues, setFormValues } = useStepper();
  const { currentTheme } = useTheme();

  const allowedGrantTypeOptions = [
    { value: 'authorization_code', label: 'Authorization Code' },
    { value: 'implicit', label: 'Implicit' },
    { value: 'hybrid', label: 'Hybrid' },
    { value: 'client_credentials', label: 'Client Credentials' },
    { value: 'refresh_token', label: 'Refresh Token' },
  ];

  return (
    <MultiStepForm<CreateClientStep2DTO, typeof schema>
      id="create-client-step2"
      onSubmit={async (values: any) => {
        setFormValues(values);
        onFormCompleted();
      }}
      schema={schema}
      options={{
        defaultValues: {
          allowedGrantTypes: ['authorization_code'],
          requirePkce: true,
          ...formValues,
        },
      }}
    >
      {({ register, formState, control }) => (
        <>
          <InputField
            label="Always Include User Claims In Id Token"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('alwaysIncludeUserClaimsInIdToken')}
          />
          <CustomInputField
            label="Allowed Grant Types"
            error={{
              name: 'allowedGrantTypes',
              errors: formState.errors,
            }}
            customInputField={
              <Controller
                control={control}
                defaultValue={['authorization_code']}
                name="allowedGrantTypes"
                render={({ field: { onChange, ref } }) => (
                  <CreatableSelect
                    ref={ref}
                    defaultValue={{ value: 'authorization_code', label: 'Authorization Code' }}
                    isMulti
                    onChange={(val) => onChange(val.map((c) => c.value))}
                    options={allowedGrantTypeOptions}
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
            }
          />
          <InputField
            label="Require PKCE"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('requirePkce')}
          />
          <InputField
            label="Allow Plain Text PKCE"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('allowPlainTextPkce')}
          />
          <InputField
            label="Require Request Object"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('requireRequestObject')}
          />
          <InputField
            label="Allow Access Tokens Via Browser"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('allowAccessTokensViaBrowser')}
          />
          <CustomInputField
            label="Redirect Uris"
            error={{
              name: 'redirectUris',
              errors: formState.errors,
            }}
            customInputField={
              <Controller
                control={control}
                name="redirectUris"
                render={({ field: { onChange, ref } }) => (
                  <CreatableSelect
                    ref={ref}
                    isMulti
                    onChange={(val: any) =>
                      onChange(val.map((c: { value: string; label: string }) => c.value))
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
            }
          />
          <CustomInputField
            label="Post Logout Redirect Uris"
            error={{
              name: 'postLogoutRedirectUris',
              errors: formState.errors,
            }}
            customInputField={
              <Controller
                control={control}
                name="postLogoutRedirectUris"
                render={({ field: { onChange, ref } }) => (
                  <CreatableSelect
                    ref={ref}
                    isMulti
                    onChange={(val: any) =>
                      onChange(val.map((c: { value: string; label: string }) => c.value))
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
            }
          />
          <InputField
            label="Front Channel Logout Uri"
            error={{
              errors: formState.errors,
            }}
            registration={register('frontChannelLogoutUri')}
          />
          <InputField
            label="Front Channel Logout Session Required"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('frontChannelLogoutSessionRequired')}
          />
        </>
      )}
    </MultiStepForm>
  );
};
