import { Button, Form, InputField } from 'eiromplays-ui';
import * as z from 'zod';

import { useConsent } from '../api/consent';
import { ConsentInputModel, ConsentViewModel } from '../types';

import { ScopesList } from './ScopesList';

const schema = z.object({
  rememberConsent: z.boolean(),
  scopesConsented: z.array(z.string()),
});

type ConsentFormProps = {
  data: ConsentViewModel;
};

export const ConsentForm = ({ data }: ConsentFormProps) => {
  const consentMutation = useConsent();

  // TODO: Find a way to set button clicked value
  let buttonClicked = '';

  return (
    <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
      <div className="flex flex-column flex-wrap gap-5 pl-5 pb-5">
        <Form<ConsentInputModel, typeof schema>
          onSubmit={async (values) => {
            values.button = buttonClicked;
            values.returnUrl = data.returnUrl;

            await consentMutation.mutateAsync({ data: values });
          }}
          options={{
            defaultValues: {
              rememberConsent: data.allowRememberConsent,
              scopesConsented: data.apiScopes
                .map((c) => c.value)
                .concat(data.identityScopes.map((c) => c.value)),
            },
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <ScopesList
                title="Personal Information"
                scopes={data.identityScopes}
                register={register}
              />
              <ScopesList title="Application Access" scopes={data.apiScopes} register={register} />
              <InputField
                label="Description"
                placeholder="Description or name of device"
                error={formState.errors['description']}
                registration={register('description')}
              />
              {data.allowRememberConsent && (
                <InputField
                  type="checkbox"
                  label="Remember My Decision"
                  error={formState.errors['rememberConsent']}
                  registration={register('rememberConsent')}
                />
              )}
              <div className="flex gap-5 justify-center items-center">
                <Button
                  onClick={() => (buttonClicked = 'yes')}
                  isLoading={consentMutation.isLoading}
                  type="submit"
                  className="w-full mt-4"
                >
                  Yes, Allow
                </Button>
                <Button
                  onClick={() => (buttonClicked = 'no')}
                  isLoading={consentMutation.isLoading}
                  type="submit"
                  variant="danger"
                  className="w-full mt-4"
                >
                  No, Do Not Allow
                </Button>
              </div>
            </>
          )}
        </Form>
      </div>
    </div>
  );
};
