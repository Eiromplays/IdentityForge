import { Component } from "react";
import { Route } from "react-router";
import { Routes} from "react-router-dom";
import Home from "../../pages/Home/Home";
import { Layout } from "../Layout/Layout";
import UserSession from "../UserSession/UserSession";
import { QueryClient, QueryClientProvider } from 'react-query'
import "./app.scss";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <QueryClientProvider client={new QueryClient()}>
        <Layout>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/user-session" element={<UserSession />} />
          </Routes>
        </Layout>
      </QueryClientProvider>
    );
  }
}
