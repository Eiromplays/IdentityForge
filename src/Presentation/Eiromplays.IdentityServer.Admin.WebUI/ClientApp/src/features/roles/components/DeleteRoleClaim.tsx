import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteRoleClaim } from '../api/deleteRoleClaim';

export type DeleteRoleClaimProps = {
  roleId: string;
  claimId: number;
};

export const DeleteRoleClaim = ({ roleId, claimId }: DeleteRoleClaimProps) => {
  const deleteRoleClaimMutation = useDeleteRoleClaim();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Role Claim"
      body="Are you sure you want to remove this claim?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteRoleClaimMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() =>
            deleteRoleClaimMutation.mutate({
              roleId: roleId,
              claimId: claimId,
            })
          }
        >
          Delete Role Claim
        </Button>
      }
    />
  );
};
