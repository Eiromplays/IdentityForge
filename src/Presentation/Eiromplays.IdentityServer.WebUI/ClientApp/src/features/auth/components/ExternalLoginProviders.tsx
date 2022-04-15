import { ExternalProvider } from '../types';

import { ExternalLoginProvider } from './ExternalProvider';

type ExternalProvidersProps = {
  externalProviders: ExternalProvider[];
};

export const ExternalLoginProviders = ({ externalProviders }: ExternalProvidersProps) => {
  //TODO: Find a better way to get the returnUrl
  let returnUrl = '';
  const idx = location.href.toLowerCase().indexOf('?returnurl=');
  if (idx > 0) {
    returnUrl = location.href.substring(idx + 11);
  }

  return (
    <div>
      {externalProviders.map((externalProvider) => (
        <div key={externalProvider.authenticationScheme}>
          <ExternalLoginProvider externalProvider={externalProvider} returnUrl={returnUrl} />
        </div>
      ))}
    </div>
  );
};
