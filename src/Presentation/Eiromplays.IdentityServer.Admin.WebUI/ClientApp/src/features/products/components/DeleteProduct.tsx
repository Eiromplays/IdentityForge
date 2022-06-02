import { Button, ConfirmationDialog } from 'eiromplays-ui';

import { useDeleteProduct } from '../api/deleteProduct';

type DeleteProductProps = {
  productId: string;
};

export const DeleteProduct = ({ productId }: DeleteProductProps) => {
  const deleteProductMutation = useDeleteProduct();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Product"
      body="Are you sure you want to delete this Product?"
      triggerButton={<Button variant="danger">Delete</Button>}
      confirmButton={
        <Button
          isLoading={deleteProductMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() => deleteProductMutation.mutate({ productId: productId })}
        >
          Delete Product
        </Button>
      }
    />
  );
};
