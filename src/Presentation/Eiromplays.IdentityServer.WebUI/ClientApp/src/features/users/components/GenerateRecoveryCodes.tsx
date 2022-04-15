import { Button } from '@/components/Elements';

import { useGenerateRecoveryCodes } from '../api/generateRecoveryCodes';

import { ShowRecoveryCodes } from './ShowRecoveryCodes';

export const GenerateRecoveryCodes = () => {
  const generateRecoveryCodesMutation = useGenerateRecoveryCodes();

  return (
    <>
      <Button
        onClick={async () => await generateRecoveryCodesMutation.mutateAsync(undefined)}
        isLoading={generateRecoveryCodesMutation.isLoading}
        variant="danger"
        size="sm"
      >
        Reset recovery code
      </Button>
      {generateRecoveryCodesMutation.data && generateRecoveryCodesMutation.data.length > 0 && (
        <ShowRecoveryCodes codes={generateRecoveryCodesMutation.data} />
      )}
    </>
  );
};
