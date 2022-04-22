import { Layout } from '../components/Layout';

export const NotAllowed = () => {
  return (
    <Layout title="Not Allowed">
      <div className="text-center">
        <h1 className="text-1xl">You are not allowed to access this page.</h1> <br />
        <p>
          Click{' '}
          <a className="underline" href="/app">
            here
          </a>{' '}
          to go back.
        </p>
      </div>
    </Layout>
  );
};
