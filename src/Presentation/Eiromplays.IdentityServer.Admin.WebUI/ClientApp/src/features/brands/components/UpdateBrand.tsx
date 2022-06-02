import { Button, ConfirmationDialog, Spinner, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { useBrand } from '../api/getBrand';
import { UpdateBrandDTO, useUpdateBrand } from '../api/updateBrand';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  description: z.string(),
});

type UpdateBrandProps = {
  brandId: string;
};

export const UpdateBrand = ({ brandId }: UpdateBrandProps) => {
  const brandQuery = useBrand({ brandId: brandId });
  const updateBrandMutation = useUpdateBrand();

  if (brandQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!brandQuery.data) return null;

  return (
    <>
      <FormDrawer
        isDone={updateBrandMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Update Brand
          </Button>
        }
        title="Update Brand"
        submitButton={
          <ConfirmationDialog
            icon="warning"
            title="Update Brand"
            body="Are you sure you want to update this Brand?"
            triggerButton={
              <Button size="sm" isLoading={updateBrandMutation.isLoading}>
                Submit
              </Button>
            }
            confirmButton={
              <Button
                form="update-brand"
                type="submit"
                className="mt-2"
                variant="warning"
                size="sm"
                isLoading={updateBrandMutation.isLoading}
              >
                Update Brand
              </Button>
            }
          />
        }
      >
        <Form<UpdateBrandDTO['data'], typeof schema>
          id="update-product"
          onSubmit={async (values) => {
            values.id = brandId;
            await updateBrandMutation.mutateAsync({
              brandId: brandId,
              data: values,
            });
          }}
          options={{
            defaultValues: {
              name: brandQuery.data?.name,
              description: brandQuery.data?.description,
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
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
