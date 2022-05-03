import { ContentLayout, useAuth } from 'eiromplays-ui';

export const Dashboard = () => {
  const { user } = useAuth();
  return (
    <ContentLayout title="Dashboard">
      <h2 className="text-xl mt-2">
        Welcome <b>{`${user?.username}`}</b>
      </h2>
      <h3 className="my-3">
        Your roles : <b>{user?.roles.join(', ')}</b>
      </h3>
      <p className="font-medium">In this application you can:</p>
      <ul className="my-4 list-inside list-disc">
        <li>Edit your profile</li>
        <li>Update/Delete your profile picture</li>
      </ul>
    </ContentLayout>
  );
};
