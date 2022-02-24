import { Component } from 'react';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <div>{this.props.children}</div>
      </div>
    );
  }
}
