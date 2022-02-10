import AccountCircle from '@mui/icons-material/AccountCircle';
import Avatar from '@mui/material/Avatar';
import CircularProgress from '@mui/material/CircularProgress';
import { useQuery } from 'react-query';
import { useNavigate } from 'react-router-dom';

export default function Profile() {
  const navigate = useNavigate();

  const {
    data: user,
    isLoading: userIsLoading,
    error: userError,
  } = useQuery<any>(['GET', '/users', {}]);

  if (userError) {
    navigate('/');
  }

  return (
    <>
      <h1>Profile:</h1>
      {userIsLoading && <CircularProgress color="secondary" />}
      {userError && <div>Something went wrong while loading user information.</div>}
      {user && !userIsLoading && !userError ? (
        <>
          <h2>UserName: {user.data.userName}</h2>
          <h2>DisplayName: {user.data.displayName}</h2>
          <h2>Email: {user.data.email}</h2>
          <h3>Id: {user.data.id}</h3>
          {!user.data.profilePicture ? (
            <AccountCircle />
          ) : (
            <Avatar alt="profile picture" src={user.data.profilePicture} />
          )}
        </>
      ) : (
        !userIsLoading && <AccountCircle />
      )}
    </>
  );
}
