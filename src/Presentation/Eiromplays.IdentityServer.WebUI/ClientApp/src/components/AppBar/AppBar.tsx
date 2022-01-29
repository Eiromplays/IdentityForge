import React from 'react';
import { useQuery } from 'react-query';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import AccountCircle from '@mui/icons-material/AccountCircle';
import Button from '@mui/material/Button';
import Tooltip from '@mui/material/Tooltip';
import MenuItem from '@mui/material/MenuItem';
import CircularProgress from '@mui/material/CircularProgress';
import Link from '@mui/material/Link';
import LoginIcon from '@mui/icons-material/Login';
import Grid from '@mui/material/Grid';

const requestHeaders: HeadersInit = new Headers();
requestHeaders.set('X-CSRF', '1');

const pages: { name: string, url: string }[] = [
  { name: 'Home', url: '/' }
];

let settings: { name: string, url: string }[] = [
  { name: 'Show User Session', url: '/user-session' },
];

const fetchUserSessionInfo = async (): Promise<any> => {
  const response = await fetch('/bff/user', { headers: requestHeaders });
  if (response.ok && response.status === 200) {
    return response.json();
  }

  return false;
}

const fetchProfilePicture = async (): Promise<any> => {
  const response = await fetch('/profilePicture', { headers: requestHeaders });
  if (response.ok && response.status === 200) {
    return response.json();
  }

  return false;
}


const ResponsiveAppBar = () => {
  const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);

  let logoutUrl = '/bff/logout';
  const { data: userSessionInfo, isLoading: userSessionInfoIsLoading, error: userSessionInfoError } = useQuery<any, ErrorConstructor>('userSessionInfo', fetchUserSessionInfo);

  const { data: profilePicture, isLoading: profilePictureIsLoading, error: profilePictureError } = useQuery<any, ErrorConstructor>('profilePicture', fetchProfilePicture);

  if (userSessionInfo) {
    logoutUrl = userSessionInfo.find((claim:any) => claim.type === "bff:logout_url")?.value ?? logoutUrl;

    const index = settings.findIndex(setting => setting.name === 'Logout');
    if (index === -1){
      settings.push({ name: 'Logout', url: logoutUrl });
    }
  }

  const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  return (
    <AppBar position="static">
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          <Typography
            variant="h6"
            noWrap
            component="div"
            sx={{ mr: 2, display: { xs: 'none', md: 'flex' } }}
          >
            LOGO
          </Typography>

          <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'left',
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: 'block', md: 'none' },
              }}
            >
              {pages.map((page) => (
                <MenuItem key={page.name} onClick={handleCloseNavMenu}>
                  <Typography textAlign="center"><Link href={page.url}>{page.name}</Link></Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>
          <Typography
            variant="h6"
            noWrap
            component="div"
            sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}
          >
            LOGO
          </Typography>
          <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
            {pages.map((page) => (
              <Button
                key={page.name}
                onClick={handleCloseNavMenu}
                sx={{ my: 2, color: 'white', display: 'block' }}
                href={page.url}
              >
                {page.name}
              </Button>
            ))}
          </Box>

          <Box sx={{ flexGrow: 0 }}>
            {userSessionInfoIsLoading && (
              <CircularProgress color="secondary" />
            )}
            {userSessionInfoError && (
              <div>Something went wrong while loading user information.</div>
            )}
            {userSessionInfo && !userSessionInfoIsLoading && !userSessionInfoError && (
              <div>
                <Tooltip title="Open settings">
                  <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                    <AccountCircle />
                  </IconButton>
                </Tooltip>
                <Menu
                  sx={{ mt: '45px' }}
                  id="menu-appbar"
                  anchorEl={anchorElUser}
                  anchorOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                  }}
                  keepMounted
                  transformOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                  }}
                  open={Boolean(anchorElUser)}
                  onClose={handleCloseUserMenu}
                >
                  {settings.map((setting) => (
                    <MenuItem key={setting.name} onClick={handleCloseUserMenu}>
                      <Typography textAlign="center"><Link href={setting.url}>{setting.name}</Link></Typography>
                    </MenuItem>
                  ))}
                </Menu>
              </div>
            )}
            {!userSessionInfo && !userSessionInfoIsLoading && !userSessionInfoError && (  
                <div>
                  <Grid container direction="row" alignItems="center">
                    <LoginIcon fontSize="small" />&nbsp;
                    <Link href='/bff/login' underline="hover" color="inherit">Login</Link>
                  </Grid>
                </div>
            )}
            {profilePictureIsLoading && (
              <CircularProgress color="secondary" />
            )}
            {profilePictureError && (
              <div>Something went wrong while loading profile picture information.</div>
            )}
            {profilePicture && !profilePictureIsLoading && !profilePictureError && (  
                <div>PP: {profilePicture}</div>
            )}
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
};
export default ResponsiveAppBar;