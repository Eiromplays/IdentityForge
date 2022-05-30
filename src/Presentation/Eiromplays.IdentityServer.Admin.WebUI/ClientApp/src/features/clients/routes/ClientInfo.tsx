import { useMatch } from '@tanstack/react-location';
import { Spinner, ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useClient } from '../api/getClient';
import { UpdateClient } from '../components/UpdateClient';

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

export const ClientInfo = () => {
  const {
    params: { clientId: id },
  } = useMatch<LocationGenerics>();

  const clientId = parseInt(id, 10);

  const clientQuery = useClient({ clientId: clientId });

  if (clientQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!clientQuery.data) return null;

  return (
    <ContentLayout title={`Client ${clientQuery.data?.clientName}`}>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              Client Information
            </h3>
            <UpdateClient clientId={clientId} />
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Details abut the client.
          </p>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <dl className="sm:divide-y sm:divide-gray-200">
            <Entry label="Enabled" value={clientQuery.data.enabled.toString()} />
            <Entry label="ClientId" value={clientQuery.data.clientId} />
            <Entry label="Name" value={clientQuery.data.clientName} />
            <Entry label="Description" value={clientQuery.data.description} />
            <Entry label="Client Uri" value={clientQuery.data.clientUri} />
            <Entry label="Logo Uri" value={clientQuery.data.logoUri} />
            <Entry label="Require Consent" value={clientQuery.data.requireConsent.toString()} />
            <Entry
              label="Allow Remember Consent"
              value={clientQuery.data.allowRememberConsent.toString()}
            />
          </dl>
        </div>
      </div>
    </ContentLayout>
  );
};
