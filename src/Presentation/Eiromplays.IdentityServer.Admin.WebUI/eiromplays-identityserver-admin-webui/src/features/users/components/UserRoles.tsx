import { Button } from 'eiromplays-ui';

type UserRolesProps = {
  id: string;
};

export const UserRoles = ({ id }: UserRolesProps) => {
  return (
    <Button type="button" onClick={() => (window.location.href = `/app/users/${id}/roles`)}>
      Roles
    </Button>
  );
};
