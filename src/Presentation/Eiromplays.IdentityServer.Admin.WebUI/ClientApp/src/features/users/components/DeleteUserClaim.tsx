import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { UserClaim } from '@/features/users';

import { useDeleteUserClaim } from '../api/deleteUserClaim';

export type DeleteUserClaimProps = {
  id: string;
  userClaim: UserClaim;
};

export const DeleteUserClaim = ({ id, userClaim }: DeleteUserClaimProps) => {
  const deleteUserClaimMutation = useDeleteUserClaim();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete UserClaim"
      body="Are you sure you want to remove this claim?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteUserClaimMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() =>
            deleteUserClaimMutation.mutate({
              userId: id,
              removeUserClaimRequest: { type: userClaim.type, value: userClaim.value },
            })
          }
        >
          Delete UserClaim
        </Button>
      }
    />
  );
};
