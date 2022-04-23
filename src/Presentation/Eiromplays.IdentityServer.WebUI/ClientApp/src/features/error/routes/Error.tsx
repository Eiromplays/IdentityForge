import { Spinner } from '@/components/Elements';
import { ContentLayout } from '@/components/Layout';

import { useError } from '../api/getError';

export const Error = () => {
  //TODO: Find a better way to do this
  let errorId = '';
  const idx = location.href.toLowerCase().indexOf('?errorid=');
  if (idx > 0) {
    errorId = location.href.substring(idx + 9);
  }

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
