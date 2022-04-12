import { HiOutlineTrash } from 'react-icons/hi';

import { Button, ConfirmationDialog } from '@/components/Elements';

import { useRevokeGrant } from '../api/revokeGrant';

type RevokeGrantProps = {
  clientId: string;
};

export const RevokeGrant = ({ clientId }: RevokeGrantProps) => {
  const revokeGrantMutation = useRevokeGrant();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Discussion"
      body="Are you sure you want to delete this discussion?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Revoke Grant
        </Button>
      }
      confirmButton={
        <Button
          isLoading={revokeGrantMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={async () => await revokeGrantMutation.mutateAsync({ clientId: clientId })}
        >
          Revoke Grant
        </Button>
      }
    />
  );
};
