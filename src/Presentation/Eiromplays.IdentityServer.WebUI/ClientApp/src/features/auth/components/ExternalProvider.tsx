import { Button, DynamicIcon } from 'eiromplays-ui';

import { identityServerUrl } from '@/utils/envVariables';

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
        startIcon={<DynamicIcon iconName={externalProvider.displayName} />}
        onClick={() =>
          (window.location.href = `${identityServerUrl}/spa/externalLogin?provider=${externalProvider.authenticationScheme}&returnUrl=${returnUrl}`)
        }
      >
        {externalProvider.displayName}
      </Button>
    </div>
  );
};
