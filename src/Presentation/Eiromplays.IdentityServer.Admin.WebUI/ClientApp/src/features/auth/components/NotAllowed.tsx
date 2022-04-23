import { Layout } from './Layout';

export const NotAllowed = () => {
  return (
    <Layout title="Not Allowed">
      <div className="text-center">
        <h1 className="text-1xl dark:text-gray-100">You are not allowed to perform this action!</h1>{' '}
        <br />
        <p className="dark:text-gray-50">
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
