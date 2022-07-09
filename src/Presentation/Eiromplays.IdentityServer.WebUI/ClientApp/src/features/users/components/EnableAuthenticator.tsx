import { Button, Form } from 'eiromplays-ui';
import React from 'react';
import * as z from 'zod';

import { useAddAuthenticator } from '../api/addAuthenticator';
import { AddAppAuthenticator } from '../components/AddAppAuthenticator';
import { EnableAuthenticatorRequest } from '../types';

import { ShowRecoveryCodes } from './ShowRecoveryCodes';

const schema = z.object({
  code: z.string().optional(),
});

export type EnableAuthenticatorProps = {
  provider?: string;
};

export const EnableAuthenticator = ({ provider }: EnableAuthenticatorProps) => {
  const addAuthenticatorMutation = useAddAuthenticator();

  return (
    <>
      {provider?.toLowerCase() === 'app' && <AddAppAuthenticator />}
      {provider && (addAuthenticatorMutation.data?.recoveryCodes || []).length <= 0 && (
        <div className="flex flex-column flex-wrap gap-5 pl-5 pb-5">
          <Form<EnableAuthenticatorRequest, typeof schema>
            id="add-authenticator"
            onSubmit={async (values) => {
              values.provider = provider;
              await addAuthenticatorMutation.mutateAsync(values);
            }}
            options={{
              defaultValues: {
                code: '',
              },
            }}
            schema={schema}
          >
            {() => (
              <>
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
        </div>
      )}
      {addAuthenticatorMutation.data &&
        addAuthenticatorMutation.data?.recoveryCodes?.length > 0 && (
          <ShowRecoveryCodes codes={addAuthenticatorMutation.data?.recoveryCodes} />
        )}
    </>
  );
};
