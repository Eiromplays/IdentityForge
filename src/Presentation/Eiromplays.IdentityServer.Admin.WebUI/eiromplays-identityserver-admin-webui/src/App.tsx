import { AppProvider } from "eiromplays-ui";
import { Landing, NotFound } from "./features/misc";


function App() {
  return (
    <AppProvider routes={[{path: '/', element: <Landing />}, {path: '*', element: <NotFound />}]}/>
  );
}

export default App;

