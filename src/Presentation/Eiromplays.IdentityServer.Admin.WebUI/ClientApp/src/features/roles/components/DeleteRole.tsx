import { Button, ConfirmationDialog } from 'eiromplays-ui';

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
      triggerButton={<Button variant="danger">Delete</Button>}
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
