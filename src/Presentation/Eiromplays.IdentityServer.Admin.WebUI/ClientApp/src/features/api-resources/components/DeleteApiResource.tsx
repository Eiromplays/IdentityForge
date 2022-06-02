import { Button, ConfirmationDialog } from 'eiromplays-ui';

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
      triggerButton={<Button variant="danger">Delete</Button>}
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
