import { useMatch } from '@tanstack/react-location';
import { Spinner, ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useApiScope } from '../api/getApiScope';
import { UpdateApiScope } from '../components/UpdateApiScope';

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

export const ApiScopeInfo = () => {
  const {
    params: { apiScopeId: id },
  } = useMatch<LocationGenerics>();

  const apiScopeId = parseInt(id, 10);

  const apiScopeQuery = useApiScope({ apiScopeId: apiScopeId });

  if (apiScopeQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!apiScopeQuery.data) return null;

  return (
    <ContentLayout title={`ApiScope ${apiScopeQuery.data?.name}`}>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              ApiScope Information
            </h3>
            <UpdateApiScope apiScopeId={apiScopeId} />
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Details abut the ApiScope.
          </p>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <dl className="sm:divide-y sm:divide-gray-200">
            <Entry label="Enabled" value={apiScopeQuery.data.enabled.toString()} />
            <Entry label="Name" value={apiScopeQuery.data.name} />
            <Entry label="DisplayName" value={apiScopeQuery.data.displayName} />
            <Entry label="Description" value={apiScopeQuery.data.description} />
            <Entry
              label="ShowInDiscoveryDocument"
              value={apiScopeQuery.data.showInDiscoveryDocument.toString()}
            />
            <Entry label="Emphasize" value={apiScopeQuery.data.emphasize.toString()} />
            <Entry label="Required" value={apiScopeQuery.data.required.toString()} />
            <Entry label="NonEditable" value={apiScopeQuery.data.nonEditable.toString()} />
          </dl>
        </div>
      </div>
    </ContentLayout>
  );
};
