import { InputField } from 'eiromplays-ui';
import { UseFormRegister } from 'react-hook-form';
import { HiOutlineExclamationCircle } from 'react-icons/hi';

import { ScopeViewModel } from '../types';

type ScopesListProps = {
  title: string;
  scopes: ScopeViewModel[];
  register: UseFormRegister<any>;
};

export const ScopesList = ({ title, scopes, register }: ScopesListProps) => {
  if (!scopes.length) {
    return null;
  }

  return (
    <div>
      <h3 className="text-2xl">{title}</h3>
      <div className="flex flex-wrap justify-start items-center gap-5">
        {scopes.map((scope) => (
          <div key={scope.displayName}>
            <InputField
              type="checkbox"
              label={`${scope.displayName} ${scope.required ? '(required)' : ''}`}
              subLabel={scope.description}
              icon={
                scope.emphasize && (
                  <HiOutlineExclamationCircle className="h-5 w-5 inline-block justify-center items-center" />
                )
              }
              value={`${scope.value}`}
              disabled={scope.required}
              registration={register('scopesConsented')}
            />
          </div>
        ))}
      </div>
    </div>
  );
};
