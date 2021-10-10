import * as React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import Test from './Test';

export default class Layout extends React.PureComponent<{}, { children?: React.ReactNode }> {
    public render() {
        return (
            <React.Fragment>
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
                <Test></Test>
            </React.Fragment>
        );
    }
}