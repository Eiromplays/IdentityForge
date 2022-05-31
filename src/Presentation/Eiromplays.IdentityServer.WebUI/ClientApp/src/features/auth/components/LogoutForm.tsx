import { useSearch } from '@tanstack/react-location';
import { Button, Spinner, Form, useAuth } from 'eiromplays-ui';
import React from 'react';

import { LocationGenerics } from '@/App';

import { logoutUser, useLogout } from '../api/logout';
import { LoggedOutViewModel } from '../types';

type LoginFormProps = {
  onSuccess: () => void;
};

export const LogoutForm = ({ onSuccess }: LoginFormProps) => {
  const { isLoggingOut } = useAuth();
  const logoutQuery = useLogout();

  return (
    <div className="flex justify-center items-center">
      {logoutQuery.isLoading && <Spinner />}
      <Form
        onSubmit={async () => {
          await logoutUser({ logoutId: logoutQuery.data?.logoutViewModel.logoutId || '' });

          onSuccess();
        }}
      >
        {() => (
          <>
            {logoutQuery.isSuccess &&
              !logoutQuery.data.logoutViewModel?.showLogoutPrompt &&
              logoutIframe(logoutQuery.data.loggedOutViewModel)}
            <div>
              {logoutQuery.isSuccess && logoutQuery.data.logoutViewModel?.showLogoutPrompt && (
                <>
                  <h1>Are you sure you want to logout of your account?</h1>
                  <Button isLoading={isLoggingOut} type="submit" className="w-full">
                    Yes
                  </Button>
                  <Button variant="danger" className="w-full">
                    No
                  </Button>
                </>
              )}
              {logoutQuery.isSuccess && !logoutQuery.data.logoutViewModel?.showLogoutPrompt && (
                <div className="text-center">
                  <h1 className="text-1xl">
                    You have been successfully logged out of your account!
                  </h1>{' '}
                  <br />
                  <p>
                    Click{' '}
                    <a
                      className="underline"
                      href={logoutQuery.data.loggedOutViewModel?.postLogoutRedirectUri}
                    >
                      here
                    </a>{' '}
                    to return to the application.
                  </p>
                </div>
              )}
            </div>
          </>
        )}
      </Form>
    </div>
  );
};

function logoutIframe(data: LoggedOutViewModel) {
  if (!data || !data.signOutIFrameUrl) return;

  const logoutIframe = document.createElement('iframe');
  logoutIframe.classList.add('hidden');
  document.body.append(logoutIframe);

  logoutIframe.src = data.signOutIFrameUrl;
}
