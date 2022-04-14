import { HiOutlinePlus } from 'react-icons/hi';
import QRCode from 'react-qr-code';
import * as z from 'zod';

import { Button, Spinner } from '@/components/Elements';
import { Form, FormDrawer, InputField } from '@/components/Form';
import { ContentLayout } from '@/components/Layout';

import { useAddAuthenticator } from '../api/addAuthenticator';
import { useEnableAuthenticator } from '../api/getEnableAuthenticator';
import { EnableAuthenticatorViewModel } from '../types';

const schema = z.object({
  code: z.string().min(6, 'Required').max(7, 'Required'),
});

type AddAuthenticatorProps = {
  onSuccess?: (data?: any) => void;
};

export const AddAuthenticator = ({ onSuccess }: AddAuthenticatorProps) => {
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
    <ContentLayout title="Configure Two-factor Authentication (2FA)">
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Scan this QRCode or use this key{' '}
            <code className="text-green-500">{enableAuthenticatorQuery.data.sharedKey}</code>
          </p>
        </div>
        <div className="border-t border-gray-200 flex gap-5 pt-5 pl-5 pb-5">
          <QRCode value={enableAuthenticatorQuery.data.authenticatorUri} />
          <FormDrawer
            isDone={addAuthenticatorMutation.isSuccess}
            triggerButton={
              <Button startIcon={<HiOutlinePlus className="h-4 w-4" />} size="sm">
                Add Authenticator
              </Button>
            }
            title="Add Authenticator"
            submitButton={
              <Button
                form="add-authenticator"
                type="submit"
                size="sm"
                isLoading={addAuthenticatorMutation.isLoading}
              >
                Submit
              </Button>
            }
          >
            <Form<EnableAuthenticatorViewModel, typeof schema>
              id="add-authenticator"
              onSubmit={async (values) => {
                const response = await addAuthenticatorMutation.mutateAsync(values);

                if (onSuccess) onSuccess(response);
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
                </>
              )}
            </Form>
          </FormDrawer>
        </div>
      </div>
    </ContentLayout>
  );
};
