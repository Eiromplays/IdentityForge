import axios from 'axios';
import { useQuery } from 'react-query';

const claimsKeys = {
  claim: ['claims']
  // claims: ['claims'],
  // claims: (id) => [...claimsKeys.claims, id]
}

const config = {
  headers: {
    'X-CSRF': '1'
  }
}

const fetchClaims = async () =>
  axios.get('/bff/user', config)
    .then((res) => res.data);


function useClaims() {
  return useQuery(
    claimsKeys.claim,
    async () => fetchClaims(),
    {
      staleTime: Infinity,
      cacheTime: Infinity
    }
  )
}

export { useClaims as default }