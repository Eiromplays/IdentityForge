import React from 'react';
import ReactDOM from 'react-dom/client';

import App from './App';
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import './index.css';
import 'eiromplays-ui/dist/style.css';

const rootElement = document.getElementById('root');

if (!rootElement) throw new Error('No root element found!');

ReactDOM.createRoot(rootElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();
