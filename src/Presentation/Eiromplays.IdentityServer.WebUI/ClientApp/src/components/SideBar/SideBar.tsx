import AccountCircle from '@mui/icons-material/AccountCircle';
import LoginIcon from '@mui/icons-material/Login';
import MenuIcon from '@mui/icons-material/Menu';
import { Avatar, Container, Grid, Link, Menu, MenuItem, Tooltip } from '@mui/material';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
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

type Setting = {
  name: string;
  url: string;
};

type Page = {
  name: string;
  url: string;
  icon: JSX.Element;
};

type SideBarProps = {
  pages: Page[];
  settings: Setting[];
  drawerWidth?: 240;
};

export const SideBar = ({ pages, settings, drawerWidth }: SideBarProps) => {
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
      <Toolbar>
        <IconButton
          color="inherit"
          aria-label="open drawer"
          edge="start"
          onClick={handleDrawerToggle}
          sx={{ mr: 2, display: { md: 'none' } }}
        >
          <MenuIcon />
        </IconButton>
        <Typography variant="h6" component="div">
          Eiromplays IdentityServer User Dashboard
        </Typography>
      </Toolbar>
      <Divider />
      <List>
        {pages.map((page: Page, index) => (
          <ListItem button key={index}>
            <ListItemIcon>{page.icon}</ListItemIcon>
            <Link href={page.url} underline="none" color="white">
              {page.name}
            </Link>
          </ListItem>
        ))}
      </List>
    </div>
  );

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar
        position="fixed"
        sx={{
          width: { md: `calc(100% - ${drawerWidth}px)` },
          ml: { md: `${drawerWidth}px` },
        }}
      >
        <Container maxWidth="xl">
          <Toolbar disableGutters>
            <IconButton
              color="inherit"
              aria-label="open drawer"
              edge="start"
              onClick={handleDrawerToggle}
              sx={{ mr: 2, display: { md: 'none' } }}
            >
              <MenuIcon />
            </IconButton>
            <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'flex' } }}></Box>
            <Box sx={{ flexGrow: 0, display: { xs: 'flex', md: 'flex' } }}>
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
                      <Typography sx={{ flexGrow: 0, display: { xs: 'none', md: 'flex' } }}>
                        {user.data.userName}
                      </Typography>
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
            display: { xs: 'block', md: 'none' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
        >
          {drawer}
        </Drawer>
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', md: 'block' },
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
