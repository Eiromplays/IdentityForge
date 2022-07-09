import { Button, Form } from 'eiromplays-ui';

import { Send2FaVerificationCodeDto } from '@/features/auth';

import { useSend2FaVerificationCode } from '../api/send2FaVerificationCode';

export type Send2FaVerificationCodeFormProps = {
  provider: string;
};

export const Send2FaVerificationCodeForm = ({ provider }: Send2FaVerificationCodeFormProps) => {
  const send2FaVerificationCode = useSend2FaVerificationCode();

  return (
    <Form<Send2FaVerificationCodeDto>
      onSubmit={async (values) => {
        values.provider = provider;
        await send2FaVerificationCode.mutateAsync({ data: values });
      }}
    >
      {() => (
        <>
          <div>
            <Button isLoading={send2FaVerificationCode.isLoading} type="submit" className="w-full">
              Submit
            </Button>
          </div>
        </>
      )}
    </Form>
  );
};
