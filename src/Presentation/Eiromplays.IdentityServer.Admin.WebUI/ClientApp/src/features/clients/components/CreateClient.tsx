import { useSearch } from '@tanstack/react-location';
import {
  Button,
  CustomInputField,
  defaultPageIndex,
  defaultPageSize,
  Form,
  FormDrawer,
  InputField,
  useSearchPagination,
  useTheme,
} from 'eiromplays-ui';
import React from 'react';
import { Controller } from 'react-hook-form';
import { HiOutlinePencil } from 'react-icons/hi';
import Select from 'react-select';
import makeAnimated from 'react-select/animated';
import CreatableSelect from 'react-select/creatable';
import * as z from 'zod';

import { LocationGenerics } from '@/App';
import { ApiScope, SearchApiScopeDTO } from '@/features/api-scopes';

import { CreateClientDTO, useCreateClient } from '../api/createClient';
import { MultiStepForm } from '../components/MultiStepForm';

const schema = z.object({
  enabled: z.boolean(),
  clientId: z.string().min(1, 'Required'),
  protocolType: z.string().min(1, 'Required'),
  requireClientSecret: z.boolean(),
  clientName: z.string().min(1, 'Required'),
  description: z.string(),
  clientUri: z.string().url('Invalid URL').optional().or(z.literal('')),
  logoUri: z.string().url('Invalid URL').optional().or(z.literal('')),
  requireConsent: z.boolean(),
  allowRememberConsent: z.boolean(),
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
  backChannelLogoutUri: z.string().url('Invalid URL').optional().or(z.literal('')),
  backChannelLogoutSessionRequired: z.boolean(),
  allowOfflineAccess: z.boolean(),
  allowedScopes: z.array(z.string()).min(1, 'Required'),
  alwaysIncludeUserClaimsInIdToken: z.boolean(),
  identityTokenLifetime: z.number().min(1, 'Required'),
  accessTokenLifetime: z.number().min(1, 'Required'),
  authorizationCodeLifetime: z.number().min(1, 'Required'),
  absoluteRefreshTokenLifetime: z.number().min(1, 'Required'),
  slidingRefreshTokenLifetime: z.number().min(1, 'Required'),
  consentLifetime: z.number().nullable(),
  refreshTokenUsage: z.enum(['OneTimeOnly', 'ReUseable']),
  updateAccessTokenClaimsOnRefresh: z.boolean(),
  refreshTokenExpiration: z.enum(['Absolute', 'Sliding']),
  accessTokenType: z.enum(['JWT', 'Reference']),
  enableLocalLogin: z.boolean(),
  identityProviderRestrictions: z.array(z.string()).optional(),
  includeJwtId: z.boolean(),
  claims: z.array(z.string()).optional(),
  alwaysSendClientClaims: z.boolean(),
  clientClaimsPrefix: z.string().optional(),
  pairwiseSubjectSalt: z.string().optional(),
  userSsoLifetime: z.number().nullable(),
  userCodeType: z.string().optional(),
  deviceCodeLifetime: z.number().nullable(),
  cibaLifetime: z.number().nullable(),
  pollInterval: z.number().nullable(),
  coordinateLifetimeWithUserSession: z.boolean().nullable(),
  allowedCorsOrigins: z.array(z.string()).optional(),
  properties: z.object({ key: z.string(), value: z.string() }).optional(),
});

const animatedComponents = makeAnimated();

export const CreateClient = () => {
  const search = useSearch<LocationGenerics>();
  const page = search.pagination?.index || defaultPageIndex;
  const pageSize = search.pagination?.size || defaultPageSize;
  const createClientMutation = useCreateClient();
  const { currentTheme } = useTheme();
  const [apiScopesKeyword, setApiScopesKeyword] = React.useState<string>('');

  const apiScopes = useSearchPagination<SearchApiScopeDTO, ApiScope>({
    searchData: { pageNumber: page, pageSize: pageSize, keyword: apiScopesKeyword },
    url: '/api-scopes/search',
    queryKeyName: ['search-api-scopes'],
  });

  const allowedGrantTypeOptions = [
    { value: 'authorization_code', label: 'Authorization Code' },
    { value: 'implicit', label: 'Implicit' },
    { value: 'hybrid', label: 'Hybrid' },
    { value: 'client_credentials', label: 'Client Credentials' },
    { value: 'refresh_token', label: 'Refresh Token' },
  ];

  const allowedScopesOptions = apiScopes.data?.data?.map((apiScope) => ({
    value: apiScope.name,
    label: apiScope.name,
  }));

  return (
    <>
      <MultiStepForm
        steps={[
          { label: 'Test', subLabel: 'Information about you' },
          { label: 'Test 2', subLabel: 'This is just a test', icon: HiOutlinePencil },
          { label: 'Test 3', subLabel: 'Yet another test' },
          { label: 'Test 4', subLabel: 'This is just test4', icon: HiOutlinePencil },
        ]}
      />
      <br />
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
              requireClientSecret: true,
              allowRememberConsent: true,
              allowedGrantTypes: ['authorization_code'],
              requirePkce: true,
            },
          }}
        >
          {({ register, formState, control }) => (
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
              <CustomInputField
                label="Allowed Grant Types"
                error={formState.errors['allowedGrantTypes']}
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
              <CustomInputField
                label="Redirect Uris"
                error={formState.errors['redirectUris']}
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
                error={formState.errors['postLogoutRedirectUris']}
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
                error={formState.errors['frontChannelLogoutUri']}
                registration={register('frontChannelLogoutUri')}
              />
              <InputField
                label="Front Channel Logout Session Required"
                type="checkbox"
                error={formState.errors['frontChannelLogoutSessionRequired']}
                registration={register('frontChannelLogoutSessionRequired')}
              />
              <InputField
                label="Back Channel Logout Uri"
                error={formState.errors['backChannelLogoutUri']}
                registration={register('backChannelLogoutUri')}
              />
              <InputField
                label="Back Channel Logout Session Required"
                type="checkbox"
                error={formState.errors['backChannelLogoutSessionRequired']}
                registration={register('backChannelLogoutSessionRequired')}
              />
              <InputField
                label="Allow Offline Access"
                type="checkbox"
                error={formState.errors['allowOfflineAccess']}
                registration={register('allowOfflineAccess')}
              />
              <CustomInputField
                label="Allowed Scopes"
                error={formState.errors['allowedScopes']}
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
                error={formState.errors['alwaysIncludeUserClaimsInIdToken']}
                registration={register('alwaysIncludeUserClaimsInIdToken')}
              />
              <InputField
                label="Identity Token Lifetime"
                type="number"
                error={formState.errors['identityTokenLifetime']}
                registration={register('identityTokenLifetime', { valueAsNumber: true })}
              />
              <CustomInputField
                label="Allowed Identity Token Signing Algorithms"
                error={formState.errors['allowedIdentityTokenSigningAlgorithms']}
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
                error={formState.errors['accessTokenLifetime']}
                registration={register('accessTokenLifetime', { valueAsNumber: true })}
              />
              <InputField
                label="Authorization Code Lifetime"
                type="number"
                error={formState.errors['authorizationCodeLifetime']}
                registration={register('authorizationCodeLifetime', { valueAsNumber: true })}
              />
              <InputField
                label="Absolute Refresh Token Lifetime"
                type="number"
                error={formState.errors['absoluteRefreshTokenLifetime']}
                registration={register('absoluteRefreshTokenLifetime', { valueAsNumber: true })}
              />
              <InputField
                label="Sliding Refresh Token Lifetime"
                type="number"
                error={formState.errors['slidingRefreshTokenLifetime']}
                registration={register('slidingRefreshTokenLifetime', { valueAsNumber: true })}
              />
              <InputField
                label="Consent Lifetime"
                type="number"
                error={formState.errors['consentLifetime']}
                registration={register('consentLifetime', { valueAsNumber: true })}
              />
              <InputField
                label="Refresh Token Usage"
                error={formState.errors['refreshTokenUsage']}
                registration={register('refreshTokenUsage')}
              />
              <InputField
                label="Update Access Token Claims On Refresh"
                type="checkbox"
                error={formState.errors['updateAccessTokenClaimsOnRefresh']}
                registration={register('updateAccessTokenClaimsOnRefresh')}
              />
              <InputField
                label="Refresh Token Expiration"
                error={formState.errors['refreshTokenExpiration']}
                registration={register('refreshTokenExpiration')}
              />
              <InputField
                label="Access Token Type"
                error={formState.errors['accessTokenType']}
                registration={register('accessTokenType')}
              />
              <InputField
                label="Enable Local Login"
                type="checkbox"
                error={formState.errors['enableLocalLogin']}
                registration={register('enableLocalLogin')}
              />
              <CustomInputField
                label="Identity Provider Restrictions"
                error={formState.errors['identityProviderRestrictions']}
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
                error={formState.errors['includeJwtId']}
                registration={register('includeJwtId')}
              />
              <InputField
                label="Always Send Client Claims"
                type="checkbox"
                error={formState.errors['alwaysSendClientClaims']}
                registration={register('alwaysSendClientClaims')}
              />
              <InputField
                label="Client Claims Prefix"
                error={formState.errors['clientClaimsPrefix']}
                registration={register('clientClaimsPrefix')}
              />
              <InputField
                label="Pairwise Subject Salt"
                error={formState.errors['pairwiseSubjectSalt']}
                registration={register('pairwiseSubjectSalt')}
              />
              <InputField
                label="Ciba Lifetime"
                type="number"
                error={formState.errors['cibaLifetime']}
                registration={register('cibaLifetime', { valueAsNumber: true })}
              />
              <InputField
                label="Polling Interval"
                type="number"
                error={formState.errors['pollInterval']}
                registration={register('pollInterval', { valueAsNumber: true })}
              />
              <InputField
                label="Coordinate Lifetime With User Sessions"
                type="number"
                error={formState.errors['coordinateLifetimeWithUserSession']}
                registration={register('coordinateLifetimeWithUserSession', {
                  valueAsNumber: true,
                })}
              />
              <CustomInputField
                label="Allowed Cors Origins"
                error={formState.errors['allowedCorsOrigins']}
                customInputField={
                  <Controller
                    control={control}
                    name="allowedCorsOrigins"
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
                label="Properties"
                error={formState.errors['properties'] as any}
                customInputField={
                  <Controller
                    control={control}
                    name="properties"
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
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
