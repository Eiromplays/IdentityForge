import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateProductDTO, useCreateProduct } from '../api/createProduct';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  description: z.string().nullable(),
  brandId: z.string().min(1, 'Required'),
  rate: z.number().min(1, 'Required'),
});

export const CreateProduct = () => {
  const createProductMutation = useCreateProduct();

  return (
    <>
      <FormDrawer
        isDone={createProductMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create Product
          </Button>
        }
        title="Create Product"
        submitButton={
          <Button
            form="create-product"
            type="submit"
            size="sm"
            isLoading={createProductMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateProductDTO['data'], typeof schema>
          id="create-product"
          onSubmit={async (values) => {
            await createProductMutation.mutateAsync({
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
              <InputField
                label="Rate"
                type="number"
                error={formState.errors['rate']}
                registration={register('rate', { valueAsNumber: true })}
              />
              <InputField
                label="Brand Id"
                error={formState.errors['brandId']}
                registration={register('brandId')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
