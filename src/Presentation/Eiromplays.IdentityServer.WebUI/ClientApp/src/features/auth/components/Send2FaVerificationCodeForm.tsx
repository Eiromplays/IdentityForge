import { Button, Form } from 'eiromplays-ui';

import { Send2FaVerificationCodeDto } from '@/features/auth';

import { useSend2FaVerificationCode } from '../api/send2FaVerificationCode';

export type Send2FaVerificationCodeFormProps = {
  provider: string;
  onSuccess?: () => void;
  className?: string;
};

export const Send2FaVerificationCodeForm = ({
  provider,
  onSuccess,
  className,
}: Send2FaVerificationCodeFormProps) => {
  const send2FaVerificationCode = useSend2FaVerificationCode();

  return (
    <Form<Send2FaVerificationCodeDto>
      className={className}
      onSubmit={async (values) => {
        values.provider = provider;
        await send2FaVerificationCode.mutateAsync({ data: values });
        if (onSuccess) onSuccess();
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
