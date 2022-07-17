import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteUserProvider } from '../api/deleteProvider';

export type DeleteUserProviderProps = {
  id: string;
  loginProvider: string;
  providerKey: string;
};

export const DeleteUserProvider = ({ id, loginProvider, providerKey }: DeleteUserProviderProps) => {
  const useDeleteUserProviderMutation = useDeleteUserProvider();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete UserProvider"
      body="Are you sure you want to remove this provider?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={useDeleteUserProviderMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() =>
            useDeleteUserProviderMutation.mutate({
              userId: id,
              removeLoginRequest: { loginProvider: loginProvider, providerKey: providerKey },
            })
          }
        >
          Delete UserProvider
        </Button>
      }
    />
  );
};
