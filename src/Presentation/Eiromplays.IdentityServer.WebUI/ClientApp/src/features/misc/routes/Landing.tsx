import Button from '@mui/material/Button';

import logo from '@/assets/logo.svg';

export const Landing = () => {
  return (
    <>
      <div className="h-[100vh] flex items-center">
        <div className="max-w-7xl mx-auto text-center py-12 px-4 sm:px-6 lg:py-16 lg:px-8">
          <h2 className="text-3xl font-extrabold tracking-tight text-white-900 sm:text-4xl">
            <span className="block">Eiromplays IdentityServer Admin WebUI</span>
          </h2>
          <img src={logo} alt="react" />
          <p>:D</p>
          <div className="mt-8 flex justify-center">
            <div className="inline-flex rounded-md shadow">
              <Button>Get started</Button>
            </div>
            <div className="ml-3 inline-flex">
              <a
                href="https://github.com/Eiromplays/IdentityServer.Admin"
                target="_blank"
                rel="noreferrer"
              >
                <Button>Github Repo</Button>
              </a>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
