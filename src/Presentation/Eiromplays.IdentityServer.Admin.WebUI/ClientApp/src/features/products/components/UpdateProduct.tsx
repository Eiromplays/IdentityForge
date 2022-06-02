import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useProduct } from '../api/getProduct';
import { useUpdateProduct, UpdateProductDTO } from '../api/updateProduct';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  description: z.string().nullable(),
  brandId: z.string().min(1, 'Required'),
  deleteCurrentImage: z.boolean(),
  rate: z.number().min(1, 'Required'),
});

type UpdateProductProps = {
  productId: string;
};

export const UpdateProduct = ({ productId }: UpdateProductProps) => {
  const productQuery = useProduct({ productId: productId });
  const updateProductMutation = useUpdateProduct();

  if (productQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!productQuery.data) return null;

  return (
    <>
      <FormDrawer
        isDone={updateProductMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update Product
          </Button>
        }
        title="Update Product"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update Product"
            body="Are you sure you want to update this Product?"
            triggerButton={
              <Button size="sm" isLoading={updateProductMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-product"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateProductMutation.isLoading}
              >
                Update Product
              </Button>
            }
          />
        }
      >
        <Form<UpdateProductDTO['data'], typeof schema>
          id="update-product"
          onSubmit={async (values) => {
            values.id = productId;
            await updateProductMutation.mutateAsync({
              productId: productId,
              data: values,
            });
          }}
          options={{
            defaultValues: {
              name: productQuery.data?.name,
              description: productQuery.data?.description,
              brandId: productQuery.data?.brand.id,
              rate: productQuery.data?.rate,
            },
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
                label="Brand Id"
                error={formState.errors['brandId']}
                registration={register('brandId')}
              />
              <InputField
                label="Rate"
                type="number"
                error={formState.errors['rate']}
                registration={register('rate', { valueAsNumber: true })}
              />
              <InputField
                label="Delete Current Image"
                type="checkbox"
                error={formState.errors['deleteCurrentImage']}
                registration={register('deleteCurrentImage')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
