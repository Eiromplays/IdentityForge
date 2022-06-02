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
  const url = `${identityServerUrl}/spa/externalLogin?provider=${
    externalProvider.authenticationScheme
  }&returnUrl=${encodeURIComponent(returnUrl || '')}`;

  return (
    <div>
      <Button
        startIcon={<DynamicIcon iconName={externalProvider.displayName} />}
        onClick={() => (window.location.href = url)}
      >
        {externalProvider.displayName}
      </Button>
    </div>
  );
};
