import { useSearch } from '@tanstack/react-location';
import {
  CustomInputField,
  defaultPageIndex,
  defaultPageSize,
  InputField,
  MultiStepForm,
  StepComponentProps,
  useSearchPagination,
  useStepper,
  useTheme,
} from 'eiromplays-ui';
import React from 'react';
import { Controller } from 'react-hook-form';
import Select from 'react-select';
import makeAnimated from 'react-select/animated';
import CreatableSelect from 'react-select/creatable';
import * as z from 'zod';

import { LocationGenerics } from '@/App';
import { ApiScope, SearchApiScopeDTO } from '@/features/api-scopes';

import { CreateClientDTO } from '../api/createClient';

export type CreateClientStep3DTO = Pick<
  CreateClientDTO['data'],
  | 'backChannelLogoutUri'
  | 'backChannelLogoutSessionRequired'
  | 'allowOfflineAccess'
  | 'allowedScopes'
  | 'alwaysIncludeUserClaimsInIdToken'
  | 'identityTokenLifetime'
  | 'allowedIdentityTokenSigningAlgorithms'
  | 'accessTokenLifetime'
  | 'authorizationCodeLifetime'
  | 'absoluteRefreshTokenLifetime'
>;

const schema = z.object({
  backChannelLogoutUri: z.string().url('Invalid URL').optional().or(z.literal('')),
  backChannelLogoutSessionRequired: z.boolean(),
  allowOfflineAccess: z.boolean(),
  allowedScopes: z.array(z.string()).min(1, 'Required'),
  alwaysIncludeUserClaimsInIdToken: z.boolean(),
  identityTokenLifetime: z.number().min(1, 'Required'),
  allowedIdentityTokenSigningAlgorithms: z.array(z.string()).nullable().optional(),
  accessTokenLifetime: z.number().min(1, 'Required'),
  authorizationCodeLifetime: z.number().min(1, 'Required'),
  absoluteRefreshTokenLifetime: z.number().min(1, 'Required'),
});
const animatedComponents = makeAnimated();

export const CreateClientStep3 = ({ onFormCompleted }: StepComponentProps) => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;
  const { formValues, setFormValues } = useStepper();
  const { currentTheme } = useTheme();
  const [apiScopesKeyword, setApiScopesKeyword] = React.useState<string>('');

  const apiScopes = useSearchPagination<SearchApiScopeDTO, ApiScope>({
    searchData: { pageNumber: page, pageSize: pageSize, keyword: apiScopesKeyword },
    url: '/api-scopes/search',
    queryKeyName: ['search-api-scopes'],
  });

  const allowedScopesOptions = apiScopes.data?.data?.map((apiScope) => ({
    value: apiScope.name,
    label: apiScope.name,
  }));

  return (
    <MultiStepForm<CreateClientStep3DTO, typeof schema>
      id="create-client-step3"
      onSubmit={async (values: any) => {
        setFormValues(values);
        onFormCompleted();
      }}
      schema={schema}
      options={{
        defaultValues: {
          identityTokenLifetime: 300,
          accessTokenLifetime: 3600,
          authorizationCodeLifetime: 300,
          absoluteRefreshTokenLifetime: 2592000,
          ...formValues,
        },
      }}
    >
      {({ register, formState, control }) => (
        <>
          <InputField
            label="Back Channel Logout Uri"
            error={{
              errors: formState.errors,
            }}
            registration={register('backChannelLogoutUri')}
          />
          <InputField
            label="Back Channel Logout Session Required"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('backChannelLogoutSessionRequired')}
          />
          <InputField
            label="Allow Offline Access"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('allowOfflineAccess')}
          />
          <CustomInputField
            label="Allowed Scopes"
            error={{
              name: 'allowedScopes',
              errors: formState.errors,
            }}
            customInputField={
              <Controller
                control={control}
                name="allowedScopes"
                render={({ field: { onChange, ref } }) => (
                  <Select
                    ref={ref}
                    isMulti
                    onChange={(val: any) =>
                      onChange(val.map((c: { value: string; label: string }) => c.value))
                    }
                    onInputChange={(val: string) => setApiScopesKeyword(val)}
                    options={allowedScopesOptions}
                    defaultValue={
                      formValues.allowedScopes?.map((c: any) => ({
                        value: c,
                        label: c,
                      })) || []
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
            label="Always Include User Claims In Id Token"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('alwaysIncludeUserClaimsInIdToken')}
          />
          <InputField
            label="Identity Token Lifetime"
            type="number"
            error={{
              errors: formState.errors,
            }}
            registration={register('identityTokenLifetime', { valueAsNumber: true })}
          />
          <CustomInputField
            label="Allowed Identity Token Signing Algorithms"
            error={{
              name: 'allowedIdentityTokenSigningAlgorithms',
              errors: formState.errors,
            }}
            customInputField={
              <Controller
                control={control}
                name="allowedIdentityTokenSigningAlgorithms"
                render={({ field: { onChange, ref } }) => (
                  <CreatableSelect
                    ref={ref}
                    isMulti
                    onChange={(val: any) =>
                      onChange(val.map((c: { value: string; label: string }) => c.value))
                    }
                    options={[]}
                    defaultValue={
                      formValues.allowedIdentityTokenSigningAlgorithms?.map((c: string) => ({
                        value: c,
                        label: c,
                      })) || []
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
            label="Access Token Lifetime"
            type="number"
            error={{
              errors: formState.errors,
            }}
            registration={register('accessTokenLifetime', { valueAsNumber: true })}
          />
          <InputField
            label="Authorization Code Lifetime"
            type="number"
            error={{
              errors: formState.errors,
            }}
            registration={register('authorizationCodeLifetime', { valueAsNumber: true })}
          />
          <InputField
            label="Absolute Refresh Token Lifetime"
            type="number"
            error={{
              errors: formState.errors,
            }}
            registration={register('absoluteRefreshTokenLifetime', { valueAsNumber: true })}
          />
        </>
      )}
    </MultiStepForm>
  );
};
