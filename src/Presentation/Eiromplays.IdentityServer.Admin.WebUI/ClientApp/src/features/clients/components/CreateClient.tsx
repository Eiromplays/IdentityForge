import { Button, FormDrawer, Stepper } from 'eiromplays-ui';
import React from 'react';
import { HiOutlinePencil } from 'react-icons/hi';

import { useCreateClient } from '../api/createClient';

import { CreateClientStep1 } from './CreateClientStep1';
import { CreateClientStep2 } from './CreateClientStep2';
import { CreateClientStep3 } from './CreateClientStep3';
import { CreateClientStep4 } from './CreateClientStep4';
import { CreateClientStep5 } from './CreateClientStep5';

export const CreateClient = () => {
  const createClientMutation = useCreateClient();

  return (
    <>
      <FormDrawer
        isDone={createClientMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create Client
          </Button>
        }
        title="Create Client"
        size="full"
        submitButton={<></>}
      >
        <Stepper
          steps={[
            { label: 'Step 1', component: CreateClientStep1 },
            { label: 'Step 2', component: CreateClientStep2 },
            { label: 'Step 3', component: CreateClientStep3 },
            { label: 'Step 4', component: CreateClientStep4 },
            { label: 'Step 5', component: CreateClientStep5 },
          ]}
          onSubmit={async (values) => {
            await createClientMutation.mutateAsync({ data: values });
          }}
        />
      </FormDrawer>
    </>
  );
};
