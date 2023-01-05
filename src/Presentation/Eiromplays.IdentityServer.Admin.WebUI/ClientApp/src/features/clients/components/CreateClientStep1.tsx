import { Button, CustomInputField, Form, FormDrawer, InputField } from 'eiromplays-ui';
import React from 'react';
import { Controller } from 'react-hook-form';
import { HiOutlinePencil } from 'react-icons/hi';
import Select from 'react-select';
import CreatableSelect from 'react-select/creatable';

import { useMultiStepForm } from '@/lib/MultiStepForm';

import { CreateClientDTO } from '../api/createClient';

export const CreateClientStep1 = () => {
  const { formValues, setFormValues } = useMultiStepForm();

  return (
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
          </>
        )}
      </Form>
    </FormDrawer>
  );
};
