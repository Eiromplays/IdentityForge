import { useSearch } from '@tanstack/react-location';

import { LocationGenerics } from '@/App';

import { ExternalProvider } from '../types';

import { ExternalLoginProvider } from './ExternalProvider';

type ExternalProvidersProps = {
  title?: string;
  externalProviders: ExternalProvider[];
};

//TODO: fix returnUrl having a capital letter as it is a query param, perhaps add a ReturnUrl search param as well

export const ExternalLoginProviders = ({ title, externalProviders }: ExternalProvidersProps) => {
  const { returnUrl, ReturnUrl } = useSearch<LocationGenerics>();

  return (
    <div>
      {title && <h3 className="mt-3 text-center text-1xl font-extrabold text-gray-900">{title}</h3>}
      <div className="flex flex-wrap justify-center items-center">
        {externalProviders.map((externalProvider) => (
          <div key={externalProvider.authenticationScheme}>
            <ExternalLoginProvider
              externalProvider={externalProvider}
              returnUrl={returnUrl || ReturnUrl}
            />
          </div>
        ))}
      </div>
    </div>
  );
};
