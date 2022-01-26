import React from 'react';
import useClaims from '../apis/claims';

function Home() {
  const { data: claims, isLoading } = useClaims();
  let logoutUrl = claims?.find(claim => claim.type === 'bff:logout_url') 
  let nameDict = claims?.find(claim => claim.type === 'name') ||  claims?.find(claim => claim.type === 'sub');
  let username = nameDict?.value; 

  if(isLoading)
    return <div>Loading...</div>

  return ( 
    <div className="p-20">
      {
        !username ? (
          <a 
            href="/bff/login?returnUrl=/"
            className="inline-block px-4 py-2 text-base font-medium text-center text-white bg-blue-500 border border-transparent rounded-md hover:bg-opacity-75"
          >
            Login
          </a>
        ) : (
          <div className="flex-shrink-0 block">
            <div className="flex items-center">
              <div className="ml-3">
                <p className="block text-base font-medium text-blue-500 md:text-sm">{`Hi, ${username}!`}</p>
                <a 
                  href={logoutUrl?.value}
                  className="block mt-1 text-sm font-medium text-blue-200 hover:text-blue-500 md:text-xs"
                >
                  Logout
                </a>
              </div>
            </div>
          </div>
        )
      }
    </div>
  )
}

export { Home };