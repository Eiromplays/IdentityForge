import { Button } from '@/components/Elements';

import { useForgetTwoFactorClient } from '../api/forgetTwoFactorClient';

export const ForgetTwoFactorClient = () => {
  const forgetTwoFactorClientMutation = useForgetTwoFactorClient();

  return (
    <Button
      onClick={async () => await forgetTwoFactorClientMutation.mutateAsync(undefined)}
      isLoading={forgetTwoFactorClientMutation.isLoading}
      variant="primary"
      size="sm"
    >
      Forget this browser/machine
    </Button>
  );
};
