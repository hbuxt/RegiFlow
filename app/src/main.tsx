import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router';
import Home from './pages/Home';
import Signup from './pages/Signup';
import './main.css';
import { AuthenticationProvider } from './contexts/AuthenticationContext';
import Login from './pages/Login';
import AppLayout, { appLayoutLoader } from './pages/AppLayout';
import { TooltipProvider } from './components/ui/tooltip';
import { QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from './components/ui/sonner';
import CreateProject, { CreateProjectError, createProjectLoader } from './pages/CreateProject';
import AccountLayout, { AccountLayoutError, accountLayoutLoader } from './pages/AccountLayout';
import Account from './pages/Account';
import PlainLayout, { plainLayoutLoader } from './pages/PlainLayout';
import queryClient from './lib/utils/tanstack';
import ProjectLayout, { ProjectLayoutError, projectLayoutLoader } from './pages/ProjectLayout';
import ProjectOverview from './pages/ProjectOverview';

const router = createBrowserRouter([
  {
    element: <AppLayout />,
    loader: appLayoutLoader,
    hydrateFallbackElement: <div></div>,
    children: [
      {
        index: true,
        element: <Home />
      },
      {
        path: "create",
        element: <CreateProject />,
        loader: createProjectLoader,
        hydrateFallbackElement: <div></div>,
        errorElement: <CreateProjectError />
      },
      {
        path: ":id",
        element: <ProjectLayout />,
        loader: projectLayoutLoader,
        hydrateFallbackElement: <div></div>,
        errorElement: <ProjectLayoutError />,
        children: [
          {
            index: true,
            element: <ProjectOverview />
          }
        ]
      },
      {
        path: "/account",
        element: <AccountLayout />,
        loader: accountLayoutLoader,
        hydrateFallbackElement: <div></div>,
        errorElement: <AccountLayoutError />,
        children: [
          {
            index: true,
            element: <Account />
          }
        ]
      }
    ]
  },
  {
    element: <PlainLayout />,
    loader: plainLayoutLoader,
    hydrateFallbackElement: <div></div>,
    children: [
      {
        path: "/account/sign-up",
        element: <Signup />
      },
      {
        path: "/account/login",
        element: <Login />
      }
    ]
  }
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <TooltipProvider delayDuration={800}>
      <QueryClientProvider client={queryClient}>
        <AuthenticationProvider>
          <RouterProvider router={router} />
        </AuthenticationProvider>
      </QueryClientProvider>
    </TooltipProvider>
    <Toaster richColors closeButton />
  </StrictMode>
);