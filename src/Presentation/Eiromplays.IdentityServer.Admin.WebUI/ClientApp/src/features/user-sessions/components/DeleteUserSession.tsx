import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteUserSession } from '../api/deleteUserSession';

type DeleteUserSessionProps = {
  userSessionKey: string;
  currentSession?: boolean;
};

export const DeleteUserSession = ({ userSessionKey, currentSession }: DeleteUserSessionProps) => {
  const deleteUserSessionMutation = useDeleteUserSession();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete User Session"
      body={`Are you sure you want to delete this user session? ${
        currentSession ? '(It will revoke access to this application)' : ''
      }`}
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete User Session
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteUserSessionMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={async () =>
            await deleteUserSessionMutation.mutateAsync({ userSessionKey: userSessionKey })
          }
        >
          Delete User Session
        </Button>
      }
    />
  );
};
