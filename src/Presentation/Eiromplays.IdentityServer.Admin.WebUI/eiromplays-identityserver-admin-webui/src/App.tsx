import { ReactLocation, MakeGenerics } from '@tanstack/react-location';
import { AppProvider } from 'eiromplays-ui';

import { AppRoutes } from './routes';


export type LocationGenerics = MakeGenerics<{
  Search: {
    pagination?: {
      index?: number
      size?: number
    }
    filters?: {
      name?: string
    }
    desc?: boolean
  }
}>

const location = new ReactLocation<LocationGenerics>();

function App() {
  return (
    <AppProvider<LocationGenerics> location={location} routes={AppRoutes()} />
  );
}

export default App;
