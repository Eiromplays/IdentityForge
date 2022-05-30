import { Button, ConfirmationDialog } from 'eiromplays-ui';

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
      triggerButton={<Button variant="danger">Delete</Button>}
      confirmButton={
        <Button
          isLoading={deleteIdentityResourceMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() => deleteIdentityResourceMutation.mutate({ identityResourceId: identityResourceId })}
        >
          Delete IdentityResource
        </Button>
      }
    />
  );
};
