import React from 'react';

import { MultiStepFormStep, StepType } from '@/lib/MultiStepForm';

export type StepperStepType = StepType & {
  completed: boolean;
  highlighted: boolean;
  selected: boolean;
};

export type StepperProps = {
  steps: StepType[];

  currentStep: number;
};

const Stepper = ({ steps, currentStep }: StepperProps) => {
  const [stepperSteps] = React.useState(
    steps.map((step, index) => ({
      ...step,
      completed: false,
      highlighted: index === currentStep,
      selected: index <= currentStep,
    }))
  );
  const [nextStep, setNextStep] = React.useState<StepperStepType[]>([]);
  const stepsRef = React.useRef<StepperStepType[]>(stepperSteps);

  const updateStep = (step: number, steps: any) => {
    const newSteps = [...steps];
    newSteps[step] = {
      ...newSteps[step],
      active: true,
      isCompleted: true,
    };
    return newSteps;
  };

  React.useEffect(() => {
    stepsRef.current = stepperSteps.map((step: StepperStepType, index: number) => ({
      ...step,
      completed: index < currentStep,
      highlighted: index === currentStep,
      selected: index <= currentStep,
    }));
    const current = updateStep(currentStep - 1, stepsRef.current);
    setNextStep(current);
  }, [stepperSteps, currentStep]);

  const stepsDisplay = nextStep.map((step, index) => {
    return (
      <MultiStepFormStep
        key={index}
        step={step}
        hasNextStep={index < nextStep.length - 1}
        nextStepIndex={index + 1}
      />
    );
  });

  return <div className="mx-4 p-4 flex justify-between items-center">{stepsDisplay}</div>;
};
export default Stepper;
