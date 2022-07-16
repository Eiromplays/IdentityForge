import { Button, ConfirmationDialog } from 'eiromplays-ui';
import { HiOutlineTrash } from 'react-icons/hi';

import { useDeleteApiScope } from '../api/deleteApiScope';

type DeleteIdentityResourceProps = {
  apiScopeId: number;
};

export const DeleteApiScope = ({ apiScopeId }: DeleteIdentityResourceProps) => {
  const deleteApiScopeMutation = useDeleteApiScope();

  return (
    <ConfirmationDialog
      icon="danger"
      title="Delete ApiScope"
      body="Are you sure you want to delete this ApiScope?"
      triggerButton={
        <Button variant="danger" startIcon={<HiOutlineTrash className="h-4 w-4" />}>
          Delete
        </Button>
      }
      confirmButton={
        <Button
          isLoading={deleteApiScopeMutation.isLoading}
          type="button"
          className="bg-red-600"
          onClick={() => deleteApiScopeMutation.mutate({ apiScopeId: apiScopeId })}
        >
          Delete ApiScope
        </Button>
      }
    />
  );
};
