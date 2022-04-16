import { HiOutlinePlus } from 'react-icons/hi';

import { Button, ConfirmationDialog } from '@/components/Elements';

import { EnableAuthenticator } from './EnableAuthenticator';

export const AddAuthenticator = () => {
  return (
    <>
      <ConfirmationDialog
        icon="info"
        title="Add Authenticator"
        body="INFORMATION!"
        triggerButton={
          <Button startIcon={<HiOutlinePlus />} size="sm" variant="primary">
            Add Authenticator
          </Button>
        }
        showCancelButton={false}
        confirmButton={<EnableAuthenticator />}
      />
    </>
  );
};
