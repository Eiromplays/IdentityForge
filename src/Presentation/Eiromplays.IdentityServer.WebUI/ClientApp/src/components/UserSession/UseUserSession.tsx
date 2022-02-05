import { useQuery } from 'react-query';

const requestHeaders: HeadersInit = new Headers();
requestHeaders.set('X-CSRF', '1');

const fetchUserSessionInfo = async (): Promise<any> => {
    const response = await fetch('/bff/user', { headers: requestHeaders });
    if (response.ok) {
        return response.json();
    }
}

export default function useUserSession() {
    const { data: userSessionInfo, isLoading, error } = useQuery<any, ErrorConstructor>('userSessionInfo', fetchUserSessionInfo);

    return  {
        userSessionInfo,
        isLoading,
        error
    };
}