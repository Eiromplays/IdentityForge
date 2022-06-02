import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateBrandDTO, useCreateBrand } from '../api/createBrand';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  description: z.string().nullable(),
});

export const CreateBrand = () => {
  const createBrandMutation = useCreateBrand();

  return (
    <>
      <FormDrawer
        isDone={createBrandMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create Brand
          </Button>
        }
        title="Create Brand"
        submitButton={
          <Button
            form="create-brand"
            type="submit"
            size="sm"
            isLoading={createBrandMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateBrandDTO['data'], typeof schema>
          id="create-brand"
          onSubmit={async (values) => {
            await createBrandMutation.mutateAsync({
              data: values,
            });
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="Name"
                error={formState.errors['name']}
                registration={register('name')}
              />
              <InputField
                label="Description"
                error={formState.errors['description']}
                registration={register('description')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
