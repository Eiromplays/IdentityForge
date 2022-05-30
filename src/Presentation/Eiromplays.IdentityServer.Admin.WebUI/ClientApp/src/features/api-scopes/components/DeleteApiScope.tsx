import { Button, ConfirmationDialog } from 'eiromplays-ui';

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
      triggerButton={<Button variant="danger">Delete</Button>}
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
