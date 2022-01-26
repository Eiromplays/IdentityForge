import React, { Component } from 'react';
import { QueryClient, QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';
import { Route } from 'react-router';
import { Counter } from './components/Counter';
import { FetchData } from './components/FetchData';
import { Home } from './components/Home';
import { Layout } from './components/Layout';
import './custom.css';


export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <QueryClientProvider client={new QueryClient()}>
          <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/counter' component={Counter} />
            <Route path='/fetch-data' component={FetchData} />
          </Layout>
          <ReactQueryDevtools initialIsOpen={false} />
      </QueryClientProvider>
    );
  }
}