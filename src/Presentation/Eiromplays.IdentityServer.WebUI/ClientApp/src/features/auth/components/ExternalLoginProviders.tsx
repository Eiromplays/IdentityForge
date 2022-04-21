import { ExternalProvider } from '../types';

import { ExternalLoginProvider } from './ExternalProvider';

type ExternalProvidersProps = {
  title?: string;
  externalProviders: ExternalProvider[];
};

export const ExternalLoginProviders = ({ title, externalProviders }: ExternalProvidersProps) => {
  //TODO: Find a better way to get the returnUrl
  let returnUrl = '';
  const idx = location.href.toLowerCase().indexOf('?returnurl=');
  if (idx > 0) {
    returnUrl = location.href.substring(idx + 11);
  }

  return (
    <div>
      {title && <h3 className="mt-3 text-center text-1xl font-extrabold text-gray-900">{title}</h3>}
      <div className="flex flex-wrap justify-center items-center">
        {externalProviders.map((externalProvider) => (
          <div key={externalProvider.authenticationScheme}>
            <ExternalLoginProvider externalProvider={externalProvider} returnUrl={returnUrl} />
          </div>
        ))}
      </div>
    </div>
  );
};
