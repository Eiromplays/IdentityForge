import { Button, ConfirmationDialog, useDarkMode } from 'eiromplays-ui';
import React from 'react';
import { HiOutlinePlus } from 'react-icons/hi';
import Select from 'react-select';

import { EnableAuthenticator } from './EnableAuthenticator';

const defaultOptions: AuthenticatorSelectOption[] = [{ value: 'App', label: 'App' }];

export type AuthenticatorSelectOption = {
  value: string;
  label: string;
};

export type AddAuthenticatorProps = {
  options: AuthenticatorSelectOption[];
};

export const AddAuthenticator = ({ options = [] }: AddAuthenticatorProps) => {
  const [provider, setProvider] = React.useState<string>('App');

  const { currentTheme } = useDarkMode();

  defaultOptions.forEach((option) => {
    if (!options.includes(option)) {
      options.push(option);
    }
  });

  return (
    <>
      <div className="flex flex-col gap-52">
        {options?.length > 0 && (
          <Select
            options={options}
            onChange={(value) => setProvider(value?.value || '')}
            value={{ value: provider, label: provider }}
            theme={(theme) =>
              currentTheme === 'dark'
                ? {
                    ...theme,
                    colors: {
                      ...theme.colors,
                      primary: '#0a0e17',
                      primary25: 'gray',
                      primary50: '#fff',
                      neutral0: '#0a0e17',
                    },
                  }
                : {
                    ...theme,
                    colors: {
                      ...theme.colors,
                    },
                  }
            }
          />
        )}
        <ConfirmationDialog
          icon="info"
          title="Add Authenticator"
          body="Please choose your authenticator type"
          triggerButton={
            <Button startIcon={<HiOutlinePlus />} size="sm" variant="primary">
              Add Authenticator
            </Button>
          }
          showCancelButton={false}
          confirmButton={<EnableAuthenticator provider={provider} />}
        />
      </div>
    </>
  );
};
