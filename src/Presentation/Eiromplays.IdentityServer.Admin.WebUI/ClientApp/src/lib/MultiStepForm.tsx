import React from 'react';
import create from 'zustand';

import Stepper, { StepperStepType } from './Stepper/Stepper';
import { UseContextProvider } from './Stepper/StepperContext';
import { StepperControl } from './Stepper/StepperControl';

export type StepType = {
  label: string;
  subLabel?: string;
  icon?: (props: React.SVGProps<SVGSVGElement>) => JSX.Element;
  component?: React.ReactNode;
};

export type MultiStepFormProps = {
  steps: StepType[];
};

export type MultiStepFormState = {
  currentStep: number;
  nextStep: (steps: StepType[]) => void;
  prevStep: (steps: StepType[]) => void;
  step: StepType;
};

const useMultiStepForm = create<MultiStepFormState>((set) => ({
  currentStep: 0,
  nextStep: (steps) =>
    set((state) => {
      if (state.currentStep < steps.length - 1) {
        const nextStep = state.currentStep + 1;
        return { ...state, currentStep: nextStep, step: steps[nextStep] };
      }
      return state;
    }),
  prevStep: (steps) =>
    set((state) => {
      if (state.currentStep > 0) {
        const prevStep = state.currentStep - 1;
        return { ...state, currentStep: prevStep, step: steps[prevStep] };
      }
      return state;
    }),
  step: {} as StepType,
}));

export const MultiStepForm = ({ steps }: MultiStepFormProps) => {
  const { currentStep, nextStep, prevStep, step } = useMultiStepForm((state) => state);

  return (
    <div className="mx-auto rounded-2xl bg-white pb-2 shadow-xl md:w-1/2">
      <div className="horizontal container mt-5 ">
        <Stepper steps={steps} currentStep={currentStep} />

        <div className="my-10 p-10 ">
          {step.component ?? <p className="text-black text-center">{step.label}</p>}
        </div>
      </div>

      {currentStep !== steps.length && (
        <div className="container mt-4 mb-8 flex justify-around">
          <button
            onClick={() => prevStep(steps)}
            className={`cursor-pointer rounded-xl border-2 border-slate-300 bg-white py-2 px-4 font-semibold uppercase text-slate-400 transition duration-200 ease-in-out hover:bg-slate-700 hover:text-white  ${
              currentStep === 1 ? ' cursor-not-allowed opacity-50 ' : ''
            }`}
          >
            Back
          </button>
          <button
            onClick={() =>
              currentStep === steps.length - 1 ? console.log('submit') : nextStep(steps)
            }
            className="cursor-pointer rounded-lg bg-green-500 py-2 px-4 font-semibold uppercase text-white transition duration-200 ease-in-out hover:bg-slate-700 hover:text-white"
          >
            {currentStep === steps.length - 1 ? 'Confirm' : 'Next'}
          </button>
        </div>
      )}
    </div>
  );
};

type MultiStepFormStepProps = {
  step: StepperStepType;
  hasNextStep: boolean;
  nextStepIndex: number;
};
export const MultiStepFormStep = ({ step, hasNextStep, nextStepIndex }: MultiStepFormStepProps) => {
  return (
    <div className={hasNextStep ? 'w-full flex items-center' : 'flex items-center'}>
      <div className="relative flex flex-col items-center text-teal-600">
        <div
          className={`rounded-full transition duration-500 ease-in-out border-2 border-gray-300 h-12 w-12 flex items-center justify-center py-3  ${
            step.selected ? 'bg-green-600 text-white font-bold border border-green-600 ' : ''
          }`}
        >
          {step.completed ? (
            <span className="text-white font-bold text-xl">&#10003;</span>
          ) : (
            nextStepIndex
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
};

export default MultiStepForm;
