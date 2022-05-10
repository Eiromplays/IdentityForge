import { Button } from 'eiromplays-ui';

import { useDisableAuthenticator } from '../api/disableAuthenticator';

export const DisableAuthenticator = () => {
  const disableAuthenticatorMutation = useDisableAuthenticator();

  return (
    <Button
      onClick={async () => await disableAuthenticatorMutation.mutateAsync(undefined)}
      isLoading={disableAuthenticatorMutation.isLoading}
      variant="inverse"
      size="sm"
    >
      Disable Authenticator
    </Button>
  );
};
