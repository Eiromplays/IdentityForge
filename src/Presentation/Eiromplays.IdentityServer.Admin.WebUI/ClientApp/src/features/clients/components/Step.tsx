import React from 'react';

export type StepProps = {
  step: StepType;
  active: boolean;
  hasPreviousStep: boolean;
  hasNextStep: boolean;
};

export type StepType = {
  label: string;
  subLabel?: string | React.ReactNode;
  icon?: (props: React.SVGProps<SVGSVGElement>) => JSX.Element;
};

export const Step = ({ step, active, hasPreviousStep, hasNextStep }: StepProps) => {
  console.log('step', step.label, active, hasPreviousStep, hasNextStep);
  return (
    <>
      <li
        className={`${hasNextStep || hasPreviousStep ? 'relative' : ''} ${
          active ? ' bg-gray-300 dark:bg-gray-800' : ' bg-gray-200 dark:bg-gray-700'
        } flex items-center justify-center p-4`}
      >
        {hasPreviousStep && (
          <span className="absolute hidden w-4 h-4 rotate-45 -translate-y-1/2 border border-b-0 border-l-0 bg-gray-200 dark:bg-gray-700 border-gray-200 dark:border-gray-700 sm:block -left-2 top-1/2"></span>
        )}
        {hasNextStep && (
          <span className="absolute hidden w-4 h-4 rotate-45 -translate-y-1/2 border border-b-0 border-l-0 border-gray-300 dark:border-gray-800 sm:block bg-gray-300 dark:bg-gray-800 -right-2 top-1/2"></span>
        )}

        {step.icon && <step.icon className="flex-shrink-0 mr-2 w-7 h-7" aria-hidden="true" />}

        <p className="leading-none">
          <strong className="block font-medium"> {step.label} </strong>
          {step.subLabel && <small className="mt-1"> {step.subLabel} </small>}
        </p>
      </li>
    </>
  );
};
