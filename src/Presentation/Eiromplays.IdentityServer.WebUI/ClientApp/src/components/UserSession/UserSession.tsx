import { Component } from "react";

const requestHeaders: HeadersInit = new Headers();
requestHeaders.set('X-CSRF', '1');

type MyProps = {

};

type MyState = {
  loading: boolean,
  userSessionInfo: {}
};

export class UserSession extends Component<MyProps, MyState> {
  static displayName = UserSession.name;

  constructor(props:any) {
    super(props);
    this.state = { userSessionInfo: {}, loading: true };
  }

  componentDidMount() {
    this.fetchUserSessionInfo();
  }

  static renderUserSessionTable(userSession:any) {
    return (
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Claim Type</th>
            <th>Claim Value</th>
          </tr>
        </thead>
        <tbody>
          {userSession.map((claim:any) => (
            <tr key={claim.type}>
              <td>{claim.type}</td>
              <td>{claim.value}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      UserSession.renderUserSessionTable(this.state.userSessionInfo)
    );

    return (
      <div>
        <h1 id="tabelLabel">User Session</h1>
        <p>This pages shows the current user's session.</p>
        {contents}
      </div>
    );
  }

  async fetchUserSessionInfo() {
    const response = await fetch("bff/user", {
      headers: requestHeaders
    });
    const data = await response.json();
    this.setState({ userSessionInfo: data, loading: false });
  }
}
