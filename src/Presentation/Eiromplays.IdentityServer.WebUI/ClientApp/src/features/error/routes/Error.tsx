import { useSearch } from '@tanstack/react-location';
import { Spinner, Head, Button } from 'eiromplays-ui';

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
    <>
      <Head title={`Error ${errorId}`} />
      <div className="bg-white dark:bg-black h-[100vh] flex flex-col items-center justify-center">
        <div className="max-w-7xl mx-auto text-center py-12 px-4 sm:px-6 lg:py-16 lg:px-8">
          <h2 className="text-2xl font-extrabold tracking-tight text-gray-900 sm:text-4xl">
            Sorry we are unable to process your request at this time.
            <br />
            Please try again later.
            <br />
            If the problem persists please contact support.
          </h2>
          {errorQuery.data.error && <p className="font-medium">Error: {errorQuery.data.error}</p>}
          {errorQuery.data.errorDescription && (
            <p className="font-medium">Description: {errorQuery.data.errorDescription}</p>
          )}
          {errorQuery.data.requestId && (
            <p className="font-medium">Request Id: {errorQuery.data.requestId}</p>
          )}
        </div>
        <Button
          onClick={() => (window.location.href = '/')}
          startIcon={
            <svg
              xmlns="http://www.w3.org/2000/svg"
              className="h-6 w-6"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
              />
            </svg>
          }
        >
          Back to safe ground.
        </Button>
      </div>
    </>
  );
};
