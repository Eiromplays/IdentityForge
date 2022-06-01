import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useRemoveLogin } from '../api/removeLogin';

type DeleteUserLoginProps = {
  loginProvider: string;
  providerKey: string;
};

export const RemoveUserLogin = ({ loginProvider, providerKey }: DeleteUserLoginProps) => {
  const removeLoginMutation = useRemoveLogin();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Remove User Login"
      body={'Are you sure you want to remove this login?'}
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Remove Login
        </Button>
      }
      confirmButton={
        <Button
          isLoading={removeLoginMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={async () =>
            await removeLoginMutation.mutateAsync({
              loginProvider: loginProvider,
              providerKey: providerKey,
            })
          }
        >
          Remove Login
        </Button>
      }
    />
  );
};
