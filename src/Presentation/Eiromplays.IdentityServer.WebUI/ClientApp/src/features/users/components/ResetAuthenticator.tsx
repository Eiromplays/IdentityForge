import { Button } from 'eiromplays-ui';

import { useResetAuthenticator } from '../api/resetAuthenticator';

export const ResetAuthenticator = () => {
  const resetAuthenticatorMutation = useResetAuthenticator();

  return (
    <Button
      onClick={async () => await resetAuthenticatorMutation.mutateAsync(undefined)}
      isLoading={resetAuthenticatorMutation.isLoading}
      variant="danger"
      size="sm"
    >
      Reset Authenticator
    </Button>
  );
};
