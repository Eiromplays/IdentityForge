import { Layout } from '../components/Layout';

export const Lockout = () => {
  return (
    <Layout title="Lockout">
      <div className="text-center">
        <h1 className="text-1xl">You have been locked out of your account!</h1> <br />
        <p>
          Click{' '}
          <a className="underline" href={`/bff/login?returnUrl=${window.location.pathname}`}>
            here
          </a>{' '}
          to try again.
        </p>
      </div>
    </Layout>
  );
};
