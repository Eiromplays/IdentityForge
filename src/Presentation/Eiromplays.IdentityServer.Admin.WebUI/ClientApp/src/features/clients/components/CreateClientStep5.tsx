import {
  CustomInputField,
  InputField,
  MultiStepForm,
  StepComponentProps,
  useStepper,
  useTheme,
} from 'eiromplays-ui';
import React from 'react';
import { Controller } from 'react-hook-form';
import makeAnimated from 'react-select/animated';
import CreatableSelect from 'react-select/creatable';
import * as z from 'zod';

import { CreateClientDTO } from '../api/createClient';

export type CreateClientStep5DTO = Pick<
  CreateClientDTO['data'],
  | 'clientClaimsPrefix'
  | 'pairwiseSubjectSalt'
  | 'cibaLifetime'
  | 'pollInterval'
  | 'coordinateLifetimeWithUserSession'
  | 'allowedCorsOrigins'
  | 'properties'
>;

const schema = z.object({
  clientClaimsPrefix: z.string(),
  pairwiseSubjectSalt: z.string().optional(),
  cibaLifetime: z.number().nullable(),
  pollInterval: z.number().nullable(),
  coordinateLifetimeWithUserSession: z.boolean().nullable(),
  allowedCorsOrigins: z.array(z.string()).optional(),
  properties: z.object({ key: z.string(), value: z.string() }).optional(),
});
const animatedComponents = makeAnimated();

export const CreateClientStep5 = ({ onFormCompleted }: StepComponentProps) => {
  const { formValues, setFormValues } = useStepper();
  const { currentTheme } = useTheme();

  return (
    <MultiStepForm<CreateClientStep5DTO, typeof schema>
      id="create-client-step5"
      onSubmit={async (values: any) => {
        setFormValues(values);
        onFormCompleted();
      }}
      schema={schema}
      options={{
        defaultValues: {
          cibaLifetime: 1,
          pollInterval: 1,
          coordinateLifetimeWithUserSession: true,
          ...formValues,
        },
      }}
    >
      {({ register, formState, control }) => (
        <>
          <InputField
            label="Client Claims Prefix"
            error={{
              errors: formState.errors,
            }}
            registration={register('clientClaimsPrefix')}
          />
          <InputField
            label="Pairwise Subject Salt"
            error={{
              errors: formState.errors,
            }}
            registration={register('pairwiseSubjectSalt')}
          />
          <InputField
            label="Ciba Lifetime"
            type="number"
            error={{
              errors: formState.errors,
            }}
            registration={register('cibaLifetime', { valueAsNumber: true })}
          />
          <InputField
            label="Polling Interval"
            type="number"
            error={{
              errors: formState.errors,
            }}
            registration={register('pollInterval', { valueAsNumber: true })}
          />
          <InputField
            label="Coordinate Lifetime With User Sessions"
            type="checkbox"
            error={{
              errors: formState.errors,
            }}
            registration={register('coordinateLifetimeWithUserSession')}
          />
          <CustomInputField
            label="Allowed Cors Origins"
            error={{
              name: 'allowedCorsOrigins',
              errors: formState.errors,
            }}
            customInputField={
              <Controller
                control={control}
                name="allowedCorsOrigins"
                render={({ field: { onChange, ref } }) => (
                  <CreatableSelect
                    ref={ref}
                    isMulti
                    onChange={(val: any) =>
                      onChange(val.map((c: { value: string; label: string }) => c.value))
                    }
                    components={animatedComponents as any}
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
              />
            }
          />
          <CustomInputField
            label="Properties"
            error={{
              name: 'properties',
              errors: formState.errors,
            }}
            customInputField={
              <Controller
                control={control}
                name="properties"
                render={({ field: { onChange, ref } }) => (
                  <CreatableSelect
                    ref={ref}
                    isMulti
                    onChange={(val: any) =>
                      onChange(val.map((c: { value: string; label: string }) => c.value))
                    }
                    components={animatedComponents as any}
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
              />
            }
          />
        </>
      )}
    </MultiStepForm>
  );
};
