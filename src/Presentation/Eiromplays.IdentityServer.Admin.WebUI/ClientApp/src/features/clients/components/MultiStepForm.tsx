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

    if (currentStep >= maxSteps || currentStep >= steps.length - 1) {
      setCurrentStep(currentStep);
    }
  };

  const handlePrevious = () => {
    setCurrentStep((prevStep) => prevStep - 1);

    if (currentStep === 0) {
      setCurrentStep(currentStep);
    }
  };

  if (steps.length <= 0) return null;

  return (
    <div>
      <div>
        <ol className="overflow-hidden text-sm text-gray-500 border border-gray-100 rounded-lg grid grid-cols-1 divide-x divide-gray-100 sm:grid-cols-3">
          {steps.map((step, i) => {
            const isCurrentStep = i === currentStep;
            return (
              <Step
                key={i}
                step={step}
                active={i === currentStep}
                hasNextStep={i >= 0 && isCurrentStep}
                hasPreviousStep={i > 0 && i !== maxSteps}
              />
            );
          })}
        </ol>
      </div>
      <button onClick={handleNext}>Next</button> <button onClick={handlePrevious}>Previous</button>
    </div>
  );
};
