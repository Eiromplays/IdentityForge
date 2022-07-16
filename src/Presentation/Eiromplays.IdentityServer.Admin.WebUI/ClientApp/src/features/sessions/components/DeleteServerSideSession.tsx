import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteServerSideSession } from '../api/deleteServerSideSession';

export type DeleteServerSideSessionProps = {
  serverSideSessionKey: string;
  currentSession: boolean;
};

export const DeleteServerSideSession = ({
  serverSideSessionKey,
  currentSession,
}: DeleteServerSideSessionProps) => {
  const deleteServerSideSessionMutation = useDeleteServerSideSession();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Server-side Session"
      body={'Are you sure you want to delete this server-side session?'}
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteServerSideSessionMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={async () =>
            await deleteServerSideSessionMutation.mutateAsync({
              currentSession: currentSession,
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
