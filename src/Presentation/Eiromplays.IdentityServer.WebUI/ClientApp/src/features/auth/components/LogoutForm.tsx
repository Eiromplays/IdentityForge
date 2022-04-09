import { Button, Spinner } from '@/components/Elements';
import { Form } from '@/components/Form';
import { useAuth } from '@/lib/auth';

import { LogoutDTO, logoutUser, useLogout } from '../api/logout';

type LoginFormProps = {
  onSuccess: () => void;
};

export const LogoutForm = ({ onSuccess }: LoginFormProps) => {
  const idx = location.href.toLowerCase().indexOf('?logoutid=');
  if (idx > 0) {
    // eslint-disable-next-line no-var
    var logoutId = location.href.substring(idx + 10);
  }
  const { isLoggingOut } = useAuth();
  const getLogout = useLogout();

  return (
    <div>
      {getLogout.isLoading && <Spinner />}
      <Form<LogoutDTO>
        onSubmit={async () => {
          console.log(getLogout.data);
          const response = await logoutUser({ logoutId: logoutId });
          console.log(response);
          onSuccess();
        }}
      >
        {() => (
          <>
            <div>
              <Button isLoading={isLoggingOut} type="submit" className="w-full">
                Logout2
              </Button>
            </div>
          </>
        )}
      </Form>
      <Button
        isLoading={getLogout.isLoading}
        onClick={async () => {
          await getLogout.mutateAsync({ logoutId: logoutId });
        }}
      >
        Logout
      </Button>
      {getLogout.isSuccess && !getLogout.data.prompt && showLoggedOut(getLogout.data)}
      <div className="mt-2 flex items-center justify-end">
        <div className="logged-out-page hidden" id="loggedOut">
          <h1>
            Logout
            <small>You are now logged out</small>
          </h1>

          <div id="postLogoutLink" className="hidden">
            Click <a>here</a> to return to the application.
          </div>
        </div>
      </div>
    </div>
  );
};

function showLoggedOut(data: any) {
  document.querySelector('#prompt')?.classList.add('hidden');
  document.querySelector('#loggedOut')?.classList.remove('hidden');

  if (data.iframeUrl) {
    const logoutIframe = document.createElement('iframe');
    document.body.append(logoutIframe);

    logoutIframe.src = data.iframeUrl;

    window.addEventListener('message', (e) => {
      console.log(e);
    });
  }

  if (data.postLogoutRedirectUri) {
    document.querySelector('#postLogoutLink')?.classList.remove('hidden');
    (document.querySelector('#postLogoutLink a') as HTMLLinkElement).href =
      data.postLogoutRedirectUri;
  }
}
