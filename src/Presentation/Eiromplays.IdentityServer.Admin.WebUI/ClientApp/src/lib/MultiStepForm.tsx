import React from 'react';

import Stepper from '@/lib/Stepper/Stepper';
import { UseContextProvider } from '@/lib/Stepper/StepperContext';
import { StepperControl } from '@/lib/Stepper/StepperControl';

export type StepType = {
  label: string;
  subLabel?: string | React.ReactNode;
  icon?: (props: React.SVGProps<SVGSVGElement>) => JSX.Element;

  component?: React.ReactNode;
};

export type MultiStepFormProps = {
  steps: StepType[];
};

export const MultiStepForm = ({ steps }: MultiStepFormProps) => {
  const [currentStepIndex, setCurrentStepIndex] = React.useState(0);
  const [currentStep, setCurrentStep] = React.useState(steps[currentStepIndex]);

  const handleClick = (direction: string) => {
    let newStep: number;

    if (direction?.toLowerCase() === 'next') {
      newStep = currentStepIndex + 1;
      if (newStep > steps.length - 1) {
        newStep = steps.length - 1;
      }
    } else {
      newStep = currentStepIndex - 1;
      if (newStep < 0) {
        newStep = 0;
      }
    }

    setCurrentStepIndex(newStep);
    setCurrentStep(steps[newStep]);
  };

  return (
    <div className="mx-auto rounded-2xl bg-white pb-2 shadow-xl md:w-1/2">
      {/* Stepper */}
      <div className="horizontal container mt-5 ">
        <Stepper steps={steps} currentStep={currentStepIndex} />

        <div className="my-10 p-10 ">
          <UseContextProvider>
            {currentStep.component ?? <p className="text-black text-center">{currentStep.label}</p>}
          </UseContextProvider>
        </div>
      </div>

      {/* navigation button */}
      {currentStepIndex !== steps.length && (
        <StepperControl handleClick={handleClick} currentStep={currentStepIndex} steps={steps} />
      )}
    </div>
  );
};

export default MultiStepForm;
