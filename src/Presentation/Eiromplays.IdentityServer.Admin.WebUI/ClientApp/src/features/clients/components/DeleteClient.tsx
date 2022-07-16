import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteClient } from '../api/deleteClient';

type DeleteClientProps = {
  clientId: number;
};

export const DeleteClient = ({ clientId }: DeleteClientProps) => {
  const deleteClientMutation = useDeleteClient();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Client"
      body="Are you sure you want to delete this client?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
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
