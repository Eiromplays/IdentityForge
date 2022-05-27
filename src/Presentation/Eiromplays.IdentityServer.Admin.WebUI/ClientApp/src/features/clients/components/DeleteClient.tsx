import { Button, ConfirmationDialog } from 'eiromplays-ui';

import { useDeleteClient } from '../api/deleteClient';

type DeleteRoleProps = {
  id: string;
};

export const DeleteClient = ({ id }: DeleteRoleProps) => {
  const deleteClientMutation = useDeleteClient();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Role"
      body="Are you sure you want to delete this role?"
      triggerButton={<Button variant="danger">Delete</Button>}
      confirmButton={
        <Button
          isLoading={deleteClientMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() => deleteClientMutation.mutate({ clientId: id })}
        >
          Delete Role
        </Button>
      }
    />
  );
};
