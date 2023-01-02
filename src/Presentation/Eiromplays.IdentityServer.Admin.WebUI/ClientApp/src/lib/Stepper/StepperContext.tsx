import { createContext, useContext, useState } from 'react';

const StepperContext = createContext({ userData: '', setUserData: (...args: any) => {} });

export const UseContextProvider = ({ children }: any) => {
  const [userData, setUserData] = useState('');

  return (
    <StepperContext.Provider value={{ userData, setUserData }}>{children}</StepperContext.Provider>
  );
};

export const useStepperContext = () => useContext(StepperContext);
