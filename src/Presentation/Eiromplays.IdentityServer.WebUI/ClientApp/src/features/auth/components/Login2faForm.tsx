import { Link, Button, Form, InputField, useAuth, Spinner, useDarkMode } from 'eiromplays-ui';
import React from 'react';
import Select from 'react-select';
import * as z from 'zod';

import { useGetSend2FaVerificationCode } from '@/features/auth/api/send2FaVerificationCode';
import { AuthenticatorSelectOption } from '@/features/users/components/AddAuthenticator';

const schema = z.object({
  twoFactorCode: z.string().max(7, 'Required').min(6, 'Required'),
  rememberMachine: z.boolean(),
});

type LoginValues = {
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
  const { login2fa, isLoggingIn } = useAuth();
  const getSend2FaVerificationCode = useGetSend2FaVerificationCode();
  const { currentTheme } = useDarkMode();

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

  console.log(getSend2FaVerificationCode.data);

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
      <Form<LoginValues, typeof schema>
        onSubmit={async (values) => {
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
