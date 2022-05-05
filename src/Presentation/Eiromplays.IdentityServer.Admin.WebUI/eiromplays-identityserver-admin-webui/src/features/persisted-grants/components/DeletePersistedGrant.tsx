import { HiOutlineTrash } from 'react-icons/hi';

import { Button, ConfirmationDialog } from '@/components/Elements';

import { useDeletePersistedGrant } from '../api/deletePersistedGrant';

type RevokePersistedGrantProps = {
  key: string;
};

export const DeletePersistedGrant = ({ key }: RevokePersistedGrantProps) => {
  const revokeGrantMutation = useDeletePersistedGrant();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Persisted Grant"
      body="Are you sure you want to delete this persisted grant?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete Persisted Grant
        </Button>
      }
      confirmButton={
        <Button
          isLoading={revokeGrantMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={async () => await revokeGrantMutation.mutateAsync({ persistedGrantKey: key })}
        >
          Delete Persisted Grant
        </Button>
      }
    />
  );
};
