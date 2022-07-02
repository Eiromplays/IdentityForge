import { Button } from 'eiromplays-ui';
import { HiOutlinePlus } from 'react-icons/hi';

import { identityServerUrl } from '@/utils/envVariables';

type AddUserLoginProps = {
  providerName: string;
};

export const AddUserLogin = ({ providerName }: AddUserLoginProps) => {
  return (
    <Button
      onClick={() =>
        (window.location.href = `${identityServerUrl}/api/v1/external-logins/link-login?provider=${providerName}`)
      }
      variant="primary"
      startIcon={<HiOutlinePlus className="h-4 w-4" />}
    >
      Add Login
    </Button>
  );
};
