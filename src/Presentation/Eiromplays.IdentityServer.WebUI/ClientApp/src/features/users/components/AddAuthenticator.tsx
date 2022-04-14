import * as z from 'zod';

import { Button } from '@/components/Elements';
import { Form, InputField } from '@/components/Form';

import { useAddAuthenticator } from '../api/addAuthenticator';
import { EnableAuthenticatorViewModel } from '../types';

const schema = z.object({
  code: z.string().min(6, 'Required').max(7, 'Required'),
});

export const AddAuthenticator = () => {
  const addAuthenticatorMutation = useAddAuthenticator();

  return (
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
            form="add-authenticator"
            type="submit"
            size="sm"
            isLoading={addAuthenticatorMutation.isLoading}
          >
            Submit
          </Button>
        </>
      )}
    </Form>
  );
};
