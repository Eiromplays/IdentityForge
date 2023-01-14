import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteIdentityProvider } from '../api/deleteIdentityProvider';

type DeleteIdentityProviderProps = {
  identityProviderId: number;
};

export const DeleteIdentityProvider = ({ identityProviderId }: DeleteIdentityProviderProps) => {
  const deleteIdentityProviderMutation = useDeleteIdentityProvider();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete IdentityProvider"
      body="Are you sure you want to delete this IdentityProvider?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteIdentityProviderMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() =>
            deleteIdentityProviderMutation.mutate({ identityProviderId: identityProviderId })
          }
        >
          Delete IdentityProvider
        </Button>
      }
    />
  );
};
