import { Layout } from '../components/Layout';

export const ConfirmedPhoneNumber = () => {
  return (
    <Layout title="Confirmed Phone Number">
      <div className="text-center">
        <h1 className="text-1xl">Successfully confirmed phone number for account</h1> <br />
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
