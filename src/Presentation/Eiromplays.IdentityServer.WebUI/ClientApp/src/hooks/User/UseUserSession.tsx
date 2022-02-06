import { useQuery } from 'react-query';

export default function useUserSession() {
    const { data: userSessionInfo, isLoading: userSessionInfoIsLoading, error: userSessionInfoError } = useQuery<any>(["GET", "/bff/user", {}]);

    return  {
        userSessionInfo,
        userSessionInfoIsLoading,
        userSessionInfoError
    };
}