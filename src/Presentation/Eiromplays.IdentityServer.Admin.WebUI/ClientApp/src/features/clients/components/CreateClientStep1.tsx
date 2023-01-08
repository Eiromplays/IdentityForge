import { InputField, MultiStepForm, StepComponentProps, useStepper } from 'eiromplays-ui';
import React from 'react';
import * as z from 'zod';

import { CreateClientDTO } from '../api/createClient';

export type CreateClientStep1DTO = Pick<
  CreateClientDTO['data'],
  | 'enabled'
  | 'clientId'
  | 'protocolType'
  | 'requireClientSecret'
  | 'clientName'
  | 'description'
  | 'clientUri'
  | 'logoUri'
  | 'requireConsent'
  | 'allowRememberConsent'
>;

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
});

export const CreateClientStep1 = ({ onFormCompleted }: StepComponentProps) => {
  const { formValues, setFormValues } = useStepper();

  return (
    <MultiStepForm<CreateClientStep1DTO, typeof schema>
      id="create-client-step1"
      onSubmit={async (values: any) => {
        setFormValues(values);
        onFormCompleted();
      }}
      schema={schema}
      options={{
        defaultValues: {
          enabled: true,
          protocolType: 'oidc',
          requireClientSecret: true,
          allowRememberConsent: true,
          ...formValues,
        },
      }}
    >
      {({ register, formState }) => (
        <>
          <InputField
            label="Enabled"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('enabled')}
          />
          <InputField
            label="ClientId"
            error={{
              errors: formState.errors,
            }}
            registration={register('clientId')}
          />
          <InputField
            label="Protocol Type"
            error={{
              errors: formState.errors,
            }}
            registration={register('protocolType')}
          />
          <InputField
            label="Require Client Secret"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('requireClientSecret')}
          />
          <InputField
            label="ClientName"
            error={{
              errors: formState.errors,
            }}
            registration={register('clientName')}
          />
          <InputField
            label="Description"
            error={{
              errors: formState.errors,
            }}
            registration={register('description')}
          />
          <InputField
            label="Client Uri"
            error={{
              errors: formState.errors,
            }}
            registration={register('clientUri')}
          />
          <InputField
            label="Logo Uri"
            error={{
              errors: formState.errors,
            }}
            registration={register('logoUri')}
          />
          <InputField
            label="Require Consent"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('requireConsent')}
          />
          <InputField
            label="Allow remember Consent"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('allowRememberConsent')}
          />
        </>
      )}
    </MultiStepForm>
  );
};
