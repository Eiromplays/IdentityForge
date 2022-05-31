import { useSearch } from '@tanstack/react-location';
import { Spinner, ContentLayout } from 'eiromplays-ui';

import { LocationGenerics } from '@/App';

import { useError } from '../api/getError';

export const Error = () => {
  const { errorId } = useSearch<LocationGenerics>();

  const errorQuery = useError({ errorId });

  if (errorQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!errorQuery.data) return null;

  return (
    <ContentLayout title={`Error ${errorId}`}>
      <h2 className="text-xl mt-2">Sorry there was an error</h2>
      {errorQuery.data.error && <p className="font-medium">: {errorQuery.data.error}</p>}
      {errorQuery.data.errorDescription && (
        <p className="font-medium">{errorQuery.data.errorDescription}</p>
      )}
      {errorQuery.data.requestId && (
        <p className="font-medium">Request Id: {errorQuery.data.requestId}</p>
      )}
    </ContentLayout>
  );
};
