import { AppProvider } from "@/providers/app";
import { Landing, NotFound } from "./features/misc";
import { AppRoutes } from "./routes";

function App() {
  return (
    <AppProvider routes={[{path: '/', element: <Landing />}, {path: '*', element: <NotFound />}, ...AppRoutes]} />
  );
}

export default App;

