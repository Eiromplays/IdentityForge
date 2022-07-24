import { useMatch } from '@tanstack/react-location';
import { Spinner, ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useApiResource } from '../api/getApiResource';
import { UpdateApiResource } from '../components/UpdateApiResource';

type EntryProps = {
  label: string;
  value: string;
};

const Entry = ({ label, value }: EntryProps) => (
  <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
    <dt className="text-sm font-medium text-gray-500 dark:text-white">{label}</dt>
    <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">{value}</dd>
  </div>
);

export const ApiResourceInfo = () => {
  const {
    params: { apiResourceId: id },
  } = useMatch<LocationGenerics>();

  const apiResourceId = +id;

  const apiResourceQuery = useApiResource({ apiResourceId: apiResourceId });

  if (apiResourceQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!apiResourceQuery.data) return null;

  return (
    <ContentLayout title={`ApiResource ${apiResourceQuery.data?.name}`}>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              ApiResource Information
            </h3>
            <UpdateApiResource apiResourceId={apiResourceId} />
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Details abut the ApiResource.
          </p>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <dl className="sm:divide-y sm:divide-gray-200">
            <Entry label="Enabled" value={apiResourceQuery.data.enabled.toString()} />
            <Entry label="Name" value={apiResourceQuery.data.name} />
            <Entry label="DisplayName" value={apiResourceQuery.data.displayName} />
            <Entry label="Description" value={apiResourceQuery.data.description} />
            <Entry
              label="ShowInDiscoveryDocument"
              value={apiResourceQuery.data.showInDiscoveryDocument.toString()}
            />
            <Entry
              label="AllowedAccessTokenSigningAlgorithms"
              value={apiResourceQuery.data.allowedAccessTokenSigningAlgorithms}
            />
            <Entry
              label="RequireResourceIndicator"
              value={apiResourceQuery.data.requireResourceIndicator.toString()}
            />
            <Entry label="NonEditable" value={apiResourceQuery.data.nonEditable.toString()} />
          </dl>
        </div>
      </div>
    </ContentLayout>
  );
};
