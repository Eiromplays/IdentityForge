import React, { Component } from "react";
import { Route } from "react-router";
import { Routes } from "react-router-dom";
import { Home } from "./components/Home";
import { Layout } from "./components/Layout";
import { UserSession } from "./components/UserSession";
import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/user-session" element={<UserSession />} />
          </Routes>
      </Layout>
    );
  }
}
