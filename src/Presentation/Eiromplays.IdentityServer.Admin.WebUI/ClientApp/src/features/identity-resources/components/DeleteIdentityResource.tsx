import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteIdentityResource } from '../api/deleteIdentityResource';

type DeleteIdentityResourceProps = {
  identityResourceId: number;
};

export const DeleteIdentityResource = ({ identityResourceId }: DeleteIdentityResourceProps) => {
  const deleteIdentityResourceMutation = useDeleteIdentityResource();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete IdentityResource"
      body="Are you sure you want to delete this IdentityResource?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteIdentityResourceMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() =>
            deleteIdentityResourceMutation.mutate({ identityResourceId: identityResourceId })
          }
        >
          Delete IdentityResource
        </Button>
      }
    />
  );
};
