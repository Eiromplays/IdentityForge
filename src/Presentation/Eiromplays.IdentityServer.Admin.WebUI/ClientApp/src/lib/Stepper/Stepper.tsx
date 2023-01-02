import React from 'react';

import { StepType } from '@/lib/MultiStepForm';

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
      <div
        key={index}
        className={index !== nextStep.length - 1 ? 'w-full flex items-center' : 'flex items-center'}
      >
        <div className="relative flex flex-col items-center text-teal-600">
          <div
            className={`rounded-full transition duration-500 ease-in-out border-2 border-gray-300 h-12 w-12 flex items-center justify-center py-3  ${
              step.selected ? 'bg-green-600 text-white font-bold border border-green-600 ' : ''
            }`}
          >
            {step.completed ? (
              <span className="text-white font-bold text-xl">&#10003;</span>
            ) : (
              index + 1
            )}
          </div>
          <div
            className={`absolute top-0  text-center mt-16 w-32 text-xs font-medium uppercase ${
              step.highlighted ? 'text-gray-900' : 'text-gray-400'
            }`}
          >
            {/*step.icon && <step.icon className="flex-shrink-0 mr-2 w-7 h-7" aria-hidden="true" />*/}
            {step.label}
            {step.subLabel && (
              <div className={step.highlighted ? 'text-gray-800' : 'text-gray-300'}>
                {step.subLabel}
              </div>
            )}
          </div>
        </div>
        <div
          className={`flex-auto border-t-2 transition duration-500 ease-in-out  ${
            step.completed ? 'border-green-600' : 'border-gray-300 '
          }`}
        ></div>
      </div>
    );
  });

  return <div className="mx-4 p-4 flex justify-between items-center">{stepsDisplay}</div>;
};
export default Stepper;
