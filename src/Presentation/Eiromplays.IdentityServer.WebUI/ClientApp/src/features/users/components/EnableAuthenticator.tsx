import { Button, Spinner, Form, InputField } from 'eiromplays-ui';
import QRCode from 'react-qr-code';
import * as z from 'zod';

import { useAddAuthenticator } from '../api/addAuthenticator';
import { useEnableAuthenticator } from '../api/getEnableAuthenticator';
import { EnableAuthenticatorViewModel } from '../types';

import { ShowRecoveryCodes } from './ShowRecoveryCodes';

const schema = z.object({
  code: z.string().min(6, 'Required').max(7, 'Required'),
});

export const EnableAuthenticator = () => {
  const enableAuthenticatorQuery = useEnableAuthenticator();
  const addAuthenticatorMutation = useAddAuthenticator();

  if (enableAuthenticatorQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!enableAuthenticatorQuery.data) return null;

  return (
    <>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Scan this QRCode or use this key{' '}
            <code className="text-green-500">{enableAuthenticatorQuery.data.sharedKey}</code>
          </p>
        </div>
        <div className="flex flex-column flex-wrap gap-5 pl-5 pb-5">
          <QRCode value={enableAuthenticatorQuery.data.authenticatorUri} />
          <Form<EnableAuthenticatorViewModel, typeof schema>
            id="add-authenticator"
            onSubmit={async (values) => {
              await addAuthenticatorMutation.mutateAsync(values);
            }}
            options={{
              defaultValues: {
                code: '',
              },
            }}
            schema={schema}
          >
            {({ register, formState }) => (
              <>
                <InputField
                  label="Code"
                  error={formState.errors['code']}
                  registration={register('code')}
                />
                <Button
                  className="mt-5"
                  form="add-authenticator"
                  type="submit"
                  size="sm"
                  isLoading={addAuthenticatorMutation.isLoading}
                >
                  Add Authenticator
                </Button>
              </>
            )}
          </Form>
          {addAuthenticatorMutation.data && addAuthenticatorMutation.data.length > 0 && (
            <ShowRecoveryCodes codes={addAuthenticatorMutation.data} />
          )}
        </div>
      </div>
    </>
  );
};
