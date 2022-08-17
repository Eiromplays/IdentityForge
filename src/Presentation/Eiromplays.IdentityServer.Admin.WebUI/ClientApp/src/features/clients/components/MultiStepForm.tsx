import React from 'react';

import { Step, StepType } from './Step';

export type MultiStepFormProps = {
  steps: StepType[];
  maxSteps?: number;
};

export const MultiStepForm = ({ steps, maxSteps = 10 }: MultiStepFormProps) => {
  const [currentStep, setCurrentStep] = React.useState(0);

  const handleNext = () => {
    setCurrentStep((prevStep) => prevStep + 1);

    if (currentStep === maxSteps) {
      setCurrentStep(currentStep);
    }
  };

  if (steps.length <= 0) return null;

  return (
    <>
      <div>
        <div>
          <ol className="grid grid-cols-1 overflow-hidden text-sm text-gray-600 dark:text-gray-100 border border-gray-200 dark:border-gray-700 divide-x divide-gray-200 dark:divide-gray-700 rounded-lg sm:grid-cols-3">
            {steps.map((step, i) => (
              <Step key={i} step={step} active={i === currentStep} previous={i < currentStep} />
            ))}
          </ol>
        </div>
      </div>
    </>
  );
};
