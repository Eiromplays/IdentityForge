import { Button, DynamicIcon } from 'eiromplays-ui';

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
        startIcon={<DynamicIcon icon={`fa/Fa${externalProvider.displayName}`} />}
        onClick={() =>
          (window.location.href = `https://localhost:7001/spa/externalLogin?provider=${externalProvider.authenticationScheme}&returnUrl=${returnUrl}`)
        }
      >
        {externalProvider.displayName}
      </Button>
    </div>
  );
};
