import { useSearch } from '@tanstack/react-location';
import { MDPreview, Spinner, Head, ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useConsent } from '../api/getConsent';
import { ConsentForm } from '../components/ConsentForm';

export const Consent = () => {
  const { returnUrl: returnUrl } = useSearch<LocationGenerics>();

  const consentQuery = useConsent({ returnUrl: encodeURIComponent(returnUrl || '') });

  if (consentQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!consentQuery.data) return null;

  return (
    <>
      <Head title={consentQuery.data.clientName} />
      <ContentLayout
        title={`${consentQuery.data.clientName} is requesting your permission`}
        subTitle="Uncheck the permissions you do not wish to grant."
        logo={consentQuery.data.clientLogoUrl}
      >
        <span className="text-xs font-bold">
          <ConsentForm data={consentQuery.data} />
        </span>
        <div className="mt-6 flex flex-col space-y-16">
          <div>
            <div className="bg-white dark:bg-lighter-black shadow overflow-hidden sm:rounded-lg">
              <div className="px-4 py-5 sm:px-6">
                <div className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
                  {consentQuery.data.description && (
                    <MDPreview value={`Description: ${consentQuery.data.description}`} />
                  )}
                  {consentQuery.data.clientUrl && (
                    <MDPreview
                      value={`<a href=${consentQuery.data.clientUrl}>${consentQuery.data.clientName}</a>`}
                    />
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </ContentLayout>
    </>
  );
};
