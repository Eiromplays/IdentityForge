// Not found, original code: https://codepen.io/Navedkhan012/pen/vrWQMY

import { useNavigate } from 'react-router-dom';

import { Button } from '@/components/Elements';

export const NotFound = () => {
  const navigate = useNavigate();

  const handleBackToSafeGround = () => {
    navigate('/');
  };

  return (
    <>
      <div className="bg-white dark:bg-black h-[100vh] flex items-center">
        <div className="max-w-7xl mx-auto text-center py-12 px-4 sm:px-6 lg:py-16 lg:px-8">
          <div>
            <div className="four_zero_four_bg">
              <h1>404</h1>
            </div>

            <div>
              <h2>Looks like you are lost</h2>
              <p>the page you are looking for is not available!</p>
            </div>
          </div>
          <div className="mt-8 flex justify-center">
            <div className="inline-flex rounded-md shadow">
              <Button
                onClick={handleBackToSafeGround}
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
          </div>
        </div>
      </div>
    </>
  );
};
