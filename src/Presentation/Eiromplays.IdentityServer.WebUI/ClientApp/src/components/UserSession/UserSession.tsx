import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Box from "@mui/material/Box";
import CircularProgress from '@mui/material/CircularProgress';
import useUserSession from './UseUserSession';

export default function UserSession() {
  const {userSessionInfo, isLoading, error} = useUserSession();

  return (
    <>
      {isLoading && (
          <div>
              <Box sx={{ display: 'flex' }}>
                <CircularProgress color="secondary" />
              </Box>
          </div>
      )}
      {error && (
        <div>Something went wrong while loading user information.</div>
      )}
      {userSessionInfo && !isLoading && !error && (
        <div>
            <h1>User Session</h1>
            <p>This pages shows the current user's session.</p>
            <TableContainer component={Paper}>
              <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                  <TableRow>
                    <TableCell>Claim Type</TableCell>
                    <TableCell align="right">Claim Value</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {userSessionInfo.map((claim:any) => (
                    <TableRow
                      key={claim.type}
                      sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                    >
                      <TableCell component="th" scope="row">
                        {claim.type}
                      </TableCell>
                      <TableCell align="right">{claim.value}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
        </div>
      )}
    </>
  );
}