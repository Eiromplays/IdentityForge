import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteApiResource } from '../api/deleteApiResource';

type DeleteIdentityResourceProps = {
  apiResourceId: number;
};

export const DeleteApiResource = ({ apiResourceId }: DeleteIdentityResourceProps) => {
  const deleteApiResourceMutation = useDeleteApiResource();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete ApiResource"
      body="Are you sure you want to delete this ApiResource?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteApiResourceMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() => deleteApiResourceMutation.mutate({ apiResourceId: apiResourceId })}
        >
          Delete ApiResource
        </Button>
      }
    />
  );
};
