import AccountCircle from '@mui/icons-material/AccountCircle';
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import CloudDownloadOutlinedIcon from '@mui/icons-material/CloudDownloadOutlined';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import LoginIcon from '@mui/icons-material/Login';
import MenuIcon from '@mui/icons-material/Menu';
import PasswordOutlinedIcon from '@mui/icons-material/PasswordOutlined';
import PersonOutlinedIcon from '@mui/icons-material/PersonOutlined';
import ScreenLockLandscapeOutlinedIcon from '@mui/icons-material/ScreenLockLandscapeOutlined';
import { Avatar, Container, Grid, Link, Menu, MenuItem, Tooltip } from '@mui/material';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import CssBaseline from '@mui/material/CssBaseline';
import Divider from '@mui/material/Divider';
import Drawer from '@mui/material/Drawer';
import IconButton from '@mui/material/IconButton';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import * as React from 'react';

import { useAuth } from '@/lib/auth';

const settings: { name: string; url: string }[] = [
  { name: 'Show User Session', url: '/user-session' },
  { name: 'Profile', url: '/profile' },
];

type Page = {
  name: string;
  url: string;
  icon: JSX.Element;
};

const drawerPages: Page[] = [
  { name: 'My Profile', url: '/profile', icon: <PersonOutlinedIcon /> },
  { name: 'Personal Data', url: '/personalData', icon: <ScreenLockLandscapeOutlinedIcon /> },
  {
    name: 'Two-factor authentication',
    url: '/two-factorAuthentication',
    icon: <LockOutlinedIcon />,
  },
  { name: 'Change Password', url: '/changePassword', icon: <PasswordOutlinedIcon /> },
  { name: 'Discovery Document', url: '/discoveryDocument', icon: <ArticleOutlinedIcon /> },
  { name: 'Persisted Grants', url: '/persistedGrants', icon: <CloudDownloadOutlinedIcon /> },
];

const drawerWidth = 240;

export const SideBar = () => {
  const [mobileOpen, setMobileOpen] = React.useState(false);
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);

  const { user } = useAuth();

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const drawer = (
    <div>
      <Toolbar />
      <Divider />
      <List>
        {drawerPages.map((page: Page, index) => (
          <ListItem button key={index}>
            <ListItemIcon>{page.icon}</ListItemIcon>
            <Link href={page.url} underline="none">
              {page.name}
            </Link>
          </ListItem>
        ))}
      </List>
    </div>
  );

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppBar
        position="static"
        sx={{
          width: { sm: `calc(100% - ${drawerWidth}px)` },
          ml: { sm: `${drawerWidth}px` },
        }}
      >
        <Container maxWidth="xl">
          <Toolbar disableGutters>
            <IconButton
              color="inherit"
              aria-label="open drawer"
              edge="start"
              onClick={handleDrawerToggle}
              sx={{ mr: 2, display: { sm: 'none' } }}
            >
              <MenuIcon />
            </IconButton>
            <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}></Box>
            <Box sx={{ flexGrow: 0, display: { xs: 'none', md: 'flex' } }}>
              {user && (
                <div>
                  <Tooltip title="Open settings">
                    <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                      {!user.data.userName ? (
                        <AccountCircle />
                      ) : (
                        <Avatar alt="profile picture" src={user.data.profilePicture} />
                      )}
                      &nbsp;&nbsp;
                      <p>{user.data.userName}</p>
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
                        <Typography textAlign="center">
                          <Link href={setting.url}>{setting.name}</Link>
                        </Typography>
                      </MenuItem>
                    ))}
                    <MenuItem key="logout" onClick={handleCloseUserMenu}>
                      <Typography textAlign="center">
                        <Link href={user.sessionInfo.logoutUrl}>Logout</Link>
                      </Typography>
                    </MenuItem>
                  </Menu>
                </div>
              )}
              {!user && (
                <div>
                  <Grid container direction="row" alignItems="center">
                    <LoginIcon fontSize="small" />
                    &nbsp;
                    <Link href="/bff/login" underline="hover" color="inherit">
                      Login
                    </Link>
                  </Grid>
                </div>
              )}
            </Box>
          </Toolbar>
        </Container>
      </AppBar>
      <Box
        component="nav"
        sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
        aria-label="mailbox folders"
      >
        {/* The implementation can be swapped with js to avoid SEO duplication of links. */}
        <Drawer
          variant="temporary"
          open={mobileOpen}
          onClose={handleDrawerToggle}
          ModalProps={{
            keepMounted: true, // Better open performance on mobile.
          }}
          sx={{
            display: { xs: 'block', sm: 'none' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
        >
          {drawer}
        </Drawer>
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', sm: 'block' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
          open
        >
          {drawer}
        </Drawer>
      </Box>
    </Box>
  );
};
