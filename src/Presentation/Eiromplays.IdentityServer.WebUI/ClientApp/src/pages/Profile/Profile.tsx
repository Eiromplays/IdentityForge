import { useQuery } from 'react-query';
import CircularProgress from '@mui/material/CircularProgress';
import toast from 'react-hot-toast';
import { useNavigate } from "react-router-dom";
import AccountCircle from '@mui/icons-material/AccountCircle';
import Avatar from '@mui/material/Avatar';

const requestHeaders: HeadersInit = new Headers();
requestHeaders.set('X-CSRF', '1');

const fetchUser = async (): Promise<any> => {
    const response = await fetch('/users', { headers: requestHeaders });

    if (response.ok) {
        return response.json();
    }

    if (response.status !== 401){
        toast.error(`Failed to fetch user: ${response.status} ${response.statusText}`);
    }
}

export default function Profile() {
    const navigate = useNavigate();

    const { data: user, isLoading: userIsLoading, error: userError } = useQuery<any, ErrorConstructor>('user', fetchUser);

    if (userError){
        navigate("/");
    }

    return (
        <>
            <h1>Profile:</h1>
            {userIsLoading && (
                <CircularProgress color="secondary" />
            )}
            {userError && (
                <div>Something went wrong while loading user information.</div>
            )}
            {user && !userIsLoading && !userError ? (  
                <>
                    <h2>UserName: {user.userName}</h2>
                    <h2>DisplayName: {user.displayName}</h2>
                    <h2>Email: {user.email}</h2>
                    <h3>Id: {user.id}</h3>
                    {!user.profilePicture ? (
                        <AccountCircle />
                    ) : <Avatar alt="profile picture" src={user.profilePicture} />}
                </>
                ) : !userIsLoading && <AccountCircle />
            }
        </>
    );
}