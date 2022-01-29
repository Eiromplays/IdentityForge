import React, { useState, useEffect } from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import AccountCircle from '@mui/icons-material/AccountCircle';
import MenuItem from '@mui/material/MenuItem';
import Menu from '@mui/material/Menu';
import { useQuery } from 'react-query'

const requestHeaders: HeadersInit = new Headers();
requestHeaders.set('X-CSRF', '1');

const fetchIsUserLoggedIn = async (): Promise<any> => {
  const response = await fetch('/bff/user', { headers: requestHeaders });
  if (response.ok && response.status === 200) {
    return response.json();
  }

  return false;
}

export default function MenuAppBar() {
  const [isUserLoggedIn, setIsUserLoggedIn] = useState(false);
  const [logoutUrl, setLogoutUrl] = useState('/bff/logout');
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const { data: userSessionInfo, isLoading, error } = useQuery<any, ErrorConstructor>('userLoggedIn', fetchIsUserLoggedIn);

  useEffect(() => {
    if (userSessionInfo === false){
      setIsUserLoggedIn(false);
    }else if (userSessionInfo) {
      const newLogoutUrl = userSessionInfo.find((claim:any) => claim.type === "bff:logout_url")?.value ?? logoutUrl;
      setIsUserLoggedIn(true);
      setLogoutUrl(newLogoutUrl);
    }
  }, [userSessionInfo, logoutUrl]);

  if (isLoading) {
    return <div>...</div>;
  }

  if (error) {
    return <div>Something went wrong</div>;
  }

  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Photos
          </Typography>
          {isUserLoggedIn && (
            <div>
              <IconButton
                size="large"
                aria-label="account of current user"
                aria-controls="menu-appbar"
                aria-haspopup="true"
                onClick={handleMenu}
                color="inherit"
              >
                <AccountCircle />
              </IconButton>
              <Menu
                id="menu-appbar"
                anchorEl={anchorEl}
                anchorOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                keepMounted
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                open={Boolean(anchorEl)}
                onClose={handleClose}
              >
                <MenuItem onClick={handleClose}>Profile</MenuItem>
                <MenuItem onClick={handleClose}>My account</MenuItem>
                <MenuItem><a href={logoutUrl}>Logout</a></MenuItem>
              </Menu>
            </div>
          )}
          {!isUserLoggedIn && (
            <div>
              <a href='/bff/login'>Login</a>
          </div>
          )}
        </Toolbar>
      </AppBar>
    </Box>
  );
}