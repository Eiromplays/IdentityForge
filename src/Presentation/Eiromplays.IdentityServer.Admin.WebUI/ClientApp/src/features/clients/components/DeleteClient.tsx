import { Button, ConfirmationDialog } from 'eiromplays-ui';

import { useDeleteClient } from '../api/deleteClient';

type DeleteRoleProps = {
  clientId: string;
};

export const DeleteClient = ({ clientId }: DeleteRoleProps) => {
  const deleteClientMutation = useDeleteClient();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Client"
      body="Are you sure you want to delete this client?"
      triggerButton={<Button variant="danger">Delete</Button>}
      confirmButton={
        <Button
          isLoading={deleteClientMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() => deleteClientMutation.mutate({ clientId: clientId })}
        >
          Delete Client
        </Button>
      }
    />
  );
};
