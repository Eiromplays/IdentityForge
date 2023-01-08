import { Button, CustomInputField, Form, InputField, Link } from 'eiromplays-ui';
import React from 'react';
import { isPossiblePhoneNumber } from 'react-phone-number-input';
import PhoneInputWithCountry from 'react-phone-number-input/react-hook-form';
import * as z from 'zod';

import {
  ResendPhoneNumberConfirmationDTO,
  useResendPhoneNumberConfirmation,
} from '../api/resendPhoneNumberConfirmation';

const schema = z.object({
  phoneNumber: z.string().min(1, 'Required').refine(isPossiblePhoneNumber, 'Invalid phone number'),
});

export type ResendPhoneNumberConfirmationFormProps = {
  onSuccess?: () => void;
};

export const ResendPhoneNumberConfirmationForm = ({
  onSuccess,
}: ResendPhoneNumberConfirmationFormProps) => {
  const resendPhoneNumberConfirmationMutation = useResendPhoneNumberConfirmation();

  return (
    <div>
      <Form<ResendPhoneNumberConfirmationDTO, typeof schema>
        onSubmit={async (values) => {
          await resendPhoneNumberConfirmationMutation.mutateAsync({ data: values });

          if (onSuccess) onSuccess();
        }}
        schema={schema}
      >
        {({ register, formState, control }) => (
          <>
            <CustomInputField
              label="Phone Number"
              error={{
                name: 'phoneNumber',
                errors: formState.errors,
              }}
              customInputField={
                <PhoneInputWithCountry
                  className="bg-white dark:bg-gray-900 block px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm',
                  'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500',
                  'dark:focus:border-indigo-900 sm:text-sm"
                  name="phoneNumber"
                  control={control}
                  register={register('phoneNumber')}
                />
              }
            />
            <div>
              <Button
                isLoading={resendPhoneNumberConfirmationMutation.isLoading}
                type="submit"
                className="w-full"
              >
                Submit
              </Button>
            </div>
          </>
        )}
      </Form>
      <div className="mt-2 gap-5 flex items-center justify-center">
        <div className="text-sm">
          <Link to="../login" className="font-medium text-blue-600 hover:text-blue-500">
            Login
          </Link>
        </div>
        <div className="text-sm">
          <Link to="../register" className="font-medium text-blue-600 hover:text-blue-500">
            Register
          </Link>
        </div>
      </div>
    </div>
  );
};
