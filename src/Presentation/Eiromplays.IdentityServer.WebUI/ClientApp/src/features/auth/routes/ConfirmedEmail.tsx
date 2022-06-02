import { Layout } from '../components/Layout';

export const ConfirmedEmail = () => {
  return (
    <Layout title="Confirmed Email">
      <div className="text-center">
        <h1 className="text-1xl">Successfully confirmed email for account</h1> <br />
        <p>
          Click{' '}
          <a className="underline" href="/auth/login">
            here
          </a>{' '}
          to login.
        </p>
      </div>
    </Layout>
  );
};
