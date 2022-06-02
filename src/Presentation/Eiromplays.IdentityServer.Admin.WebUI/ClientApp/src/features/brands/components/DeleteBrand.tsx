import { Button, ConfirmationDialog } from 'eiromplays-ui';

import { useDeleteBrand } from '../api/deleteBrand';

type DeleteBrandProps = {
  brandId: string;
};

export const DeleteBrand = ({ brandId }: DeleteBrandProps) => {
  const deleteBrandMutation = useDeleteBrand();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Brand"
      body="Are you sure you want to delete this Brand?"
      triggerButton={<Button variant="danger">Delete</Button>}
      confirmButton={
        <Button
          isLoading={deleteBrandMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() => deleteBrandMutation.mutate({ brandId: brandId })}
        >
          Delete Brand
        </Button>
      }
    />
  );
};
