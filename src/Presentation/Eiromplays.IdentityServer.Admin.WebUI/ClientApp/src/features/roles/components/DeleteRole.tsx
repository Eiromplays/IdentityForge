import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteRole } from '../api/deleteRole';

type DeleteRoleProps = {
  id: string;
};

export const DeleteRole = ({ id }: DeleteRoleProps) => {
  const deleteRoleMutation = useDeleteRole();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete Role"
      body="Are you sure you want to delete this role?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteRoleMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() => deleteRoleMutation.mutate({ roleId: id })}
        >
          Delete Role
        </Button>
      }
    />
  );
};
