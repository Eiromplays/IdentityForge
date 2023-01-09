import React from 'react';
import ReactDOM from 'react-dom/client';

import App from './App';

import './index.scss';
import 'eiromplays-ui/dist/style.css';
import 'react-phone-number-input/style.css';

const rootElement = document.getElementById('root');

if (!rootElement) throw new Error('No root element found!');

ReactDOM.createRoot(rootElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
