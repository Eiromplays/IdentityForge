import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteServerSideSession } from '../api/deleteServerSideSession';

type DeleteUserSessionProps = {
  serverSideSessionKey: string;
  currentSession?: boolean;
};

export const DeleteServerSideSession = ({
  serverSideSessionKey,
  currentSession,
}: DeleteUserSessionProps) => {
  const deleteServerSideSessionMutation = useDeleteServerSideSession();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Server-side Session"
      body={`Are you sure you want to delete this server-side session? ${
        currentSession ? '(It will revoke access to all applications using that session)' : ''
      }`}
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete Server-side Session
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteServerSideSessionMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={async () =>
            await deleteServerSideSessionMutation.mutateAsync({
              serverSideSessionKey: serverSideSessionKey,
            })
          }
        >
          Delete Server-side Session
        </Button>
      }
    />
  );
};
