import React,{useState} from "react";
import { QueryClient, QueryClientProvider, useQuery } from "react-query";

const queryClient = new QueryClient();

export default function Test() {
    return (
        <QueryClientProvider client={queryClient}>
            <Example />
        </QueryClientProvider>
    );
}

function Example() {

    const { isLoading, error, data, isFetching } = useQuery("repoData", () =>
        fetch(
        "https://api.github.com/repos/tannerlinsley/react-query"
        ).then((res) => res.json())
    );
    const [count, setCount] = useState(0);

    if (isLoading) return <>Loading...</>;
        
    if (error) return <>An error has occurred: {error}</>;

    return (
        <div>
            <button onClick={()=>setCount(count+1)}>+</button>
            <button>{count}</button>
            <h1>{data.name}</h1>
            <p>{data.description}</p>
            <strong>👀 {data.subscribers_count}</strong>{" "}
            <strong>✨ {data.stargazers_count}</strong>{" "}
            <strong>🍴 {data.forks_count}</strong>
            <div>{isFetching ? "Updating..." : ""}</div>
        </div>
    );
}