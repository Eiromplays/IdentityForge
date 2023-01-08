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

export type CreateClientStep4DTO = Pick<
  CreateClientDTO['data'],
  | 'slidingRefreshTokenLifetime'
  | 'consentLifetime'
  | 'refreshTokenUsage'
  | 'updateAccessTokenClaimsOnRefresh'
  | 'refreshTokenExpiration'
  | 'accessTokenType'
  | 'enableLocalLogin'
  | 'identityProviderRestrictions'
  | 'includeJwtId'
  | 'alwaysSendClientClaims'
>;

const schema = z.object({
  slidingRefreshTokenLifetime: z.number().min(1, 'Required'),
  consentLifetime: z.number().nullable(),
  refreshTokenUsage: z.enum(['OneTimeOnly', 'ReUseable']),
  updateAccessTokenClaimsOnRefresh: z.boolean(),
  refreshTokenExpiration: z.enum(['Absolute', 'Sliding']),
  accessTokenType: z.enum(['JWT', 'Reference']),
  enableLocalLogin: z.boolean(),
  identityProviderRestrictions: z.array(z.string()).optional(),
  includeJwtId: z.boolean(),
  alwaysSendClientClaims: z.boolean(),
});
const animatedComponents = makeAnimated();

export const CreateClientStep4 = ({ onFormCompleted }: StepComponentProps) => {
  const { formValues, setFormValues } = useStepper();
  const { currentTheme } = useTheme();

  return (
    <MultiStepForm<CreateClientStep4DTO, typeof schema>
      id="create-client-step4"
      onSubmit={async (values: any) => {
        setFormValues(values);
        onFormCompleted();
      }}
      schema={schema}
      options={{
        defaultValues: {
          slidingRefreshTokenLifetime: 1,
          consentLifetime: 1,
          refreshTokenUsage: 'OneTimeOnly',
          refreshTokenExpiration: 'Absolute',
          accessTokenType: 'JWT',
          ...formValues,
        },
      }}
    >
      {({ register, formState, control }) => (
        <>
          <InputField
            label="Sliding Refresh Token Lifetime"
            type="number"
            error={{
              errors: formState.errors,
            }}
            registration={register('slidingRefreshTokenLifetime', { valueAsNumber: true })}
          />
          <InputField
            label="Consent Lifetime"
            type="number"
            error={{
              errors: formState.errors,
            }}
            registration={register('consentLifetime', { valueAsNumber: true })}
          />
          <InputField
            label="Refresh Token Usage"
            error={{
              errors: formState.errors,
            }}
            registration={register('refreshTokenUsage')}
          />
          <InputField
            label="Update Access Token Claims On Refresh"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('updateAccessTokenClaimsOnRefresh')}
          />
          <InputField
            label="Refresh Token Expiration"
            error={{
              errors: formState.errors,
            }}
            registration={register('refreshTokenExpiration')}
          />
          <InputField
            label="Access Token Type"
            error={{
              errors: formState.errors,
            }}
            registration={register('accessTokenType')}
          />
          <InputField
            label="Enable Local Login"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('enableLocalLogin')}
          />
          <CustomInputField
            label="Identity Provider Restrictions"
            error={{
              name: 'identityProviderRestrictions',
              errors: formState.errors,
            }}
            customInputField={
              <Controller
                control={control}
                name="identityProviderRestrictions"
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
            label="Include JwtId"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('includeJwtId')}
          />
          <InputField
            label="Always Send Client Claims"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('alwaysSendClientClaims')}
          />
        </>
      )}
    </MultiStepForm>
  );
};
