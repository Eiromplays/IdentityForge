import { Component } from 'react';

import MenuAppBar from '../AppBar/AppBar';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <MenuAppBar />
        <div>{this.props.children}</div>
      </div>
    );
  }
}
