import { Link, Button, Form, InputField, useAuth, Spinner, useTheme } from 'eiromplays-ui';
import React from 'react';
import Select from 'react-select';
import * as z from 'zod';

import { AuthenticatorSelectOption } from '../../users/components/AddAuthenticator';
import { useGetValidTwoFactorProviders } from '../api/getValidTwoFactorProviders';
import { Send2FaVerificationCodeForm } from '../components/Send2FaVerificationCodeForm';

const schema = z.object({
  twoFactorCode: z.string().max(7, 'Required').min(6, 'Required'),
  rememberMachine: z.boolean(),
});

type LoginValues = {
  provider: string;
  twoFactorCode: string;
  rememberMachine: boolean;
  returnUrl?: string;
  rememberMe: boolean;
};

type LoginFormProps = {
  returnUrl?: string;
  rememberMe: boolean;
  onSuccess?: () => void;
};

const defaultOptions = [{ value: 'App', label: 'App' }];

export const Login2faForm = ({ rememberMe, returnUrl, onSuccess }: LoginFormProps) => {
  const [provider, setProvider] = React.useState<string>('App');
  const [verificationCodeSent, setVerificationCodeSent] = React.useState<boolean>(false);
  const { login2fa, isLoggingIn } = useAuth();
  const getSend2FaVerificationCode = useGetValidTwoFactorProviders();
  const { currentTheme } = useTheme();

  if (getSend2FaVerificationCode.isLoading)
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );

  const options: AuthenticatorSelectOption[] = [
    ...defaultOptions,
    ...(getSend2FaVerificationCode.data?.map((item) => ({ value: item, label: item })) || []),
  ];

  return (
    <div>
      {options?.length > 0 && (
        <Select
          options={options}
          onChange={(value) => setProvider(value?.value || '')}
          value={{ value: provider, label: provider }}
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
      {provider?.toLowerCase() !== 'app' && (
        <Send2FaVerificationCodeForm
          className="mt-4"
          provider={provider}
          onSuccess={() => setVerificationCodeSent(true)}
        />
      )}
      {provider?.toLowerCase() === 'app' ||
        (verificationCodeSent && (
          <Form<LoginValues, typeof schema>
            onSubmit={async (values) => {
              values.provider = provider;
              values.returnUrl = returnUrl;
              values.rememberMe = rememberMe;
              const response = await login2fa(values);

              if (response) onSuccess?.();
            }}
            schema={schema}
          >
            {({ register, formState }) => (
              <>
                <InputField
                  label="Two-factor code"
                  error={formState.errors['twoFactorCode']}
                  registration={register('twoFactorCode')}
                />
                <InputField
                  type="checkbox"
                  label="Remember"
                  error={formState.errors['rememberMachine']}
                  registration={register('rememberMachine')}
                />
                <div>
                  <Button isLoading={isLoggingIn} type="submit" className="w-full">
                    Log in
                  </Button>
                </div>
              </>
            )}
          </Form>
        ))}
      <div className="mt-2 flex items-center justify-end">
        <div className="text-sm">
          <Link to="../register" className="font-medium text-blue-600 hover:text-blue-500">
            Register
          </Link>
        </div>
      </div>
    </div>
  );
};
