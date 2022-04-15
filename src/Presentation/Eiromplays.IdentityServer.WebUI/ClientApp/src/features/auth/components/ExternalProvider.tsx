import { Button } from '@/components/Elements';

import { ExternalProvider } from '../types';

type ExternalLoginProviderProps = {
  externalProvider: ExternalProvider;
  returnUrl: string | undefined;
};

export const ExternalLoginProvider = ({
  externalProvider,
  returnUrl,
}: ExternalLoginProviderProps) => {
  return (
    <div>
      <Button
        onClick={() =>
          (window.location.href = `https://localhost:7001/spa/externalLogin?provider=${externalProvider.authenticationScheme}&returnUrl=${returnUrl}`)
        }
      >
        {externalProvider.displayName}
      </Button>
    </div>
  );
};
