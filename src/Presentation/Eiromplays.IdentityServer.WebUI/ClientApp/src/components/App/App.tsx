import { Component } from "react";
import { Route } from "react-router";
import { Routes} from "react-router-dom";
import Home from "../../pages/Home/Home";
import Profile from "../../pages/Profile/Profile";
import { Layout } from "../Layout/Layout";
import UserSession from "../UserSession/UserSession";
import { QueryClient, QueryClientProvider } from 'react-query';
import { Toaster } from 'react-hot-toast';
import { defaultQueryFn } from "../../api/request";
import "./app.scss";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      queryFn: defaultQueryFn,
    },
  },
});

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <QueryClientProvider client={queryClient}>
        <Layout>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/user-session" element={<UserSession />} />
            <Route path="/profile" element={<Profile />} />
          </Routes>
        </Layout>
        <Toaster position="top-right" />
      </QueryClientProvider>
    );
  }
}
