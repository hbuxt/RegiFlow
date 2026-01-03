import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Route, Routes } from 'react-router';
import Home from './pages/Home';
import Signup from './pages/Signup';
import './main.css';
import { AuthenticationProvider } from './contexts/AuthenticationContext';
import Login from './pages/Login';
import AppLayout from './pages/AppLayout';
import { TooltipProvider } from './components/ui/tooltip';
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from './components/ui/sonner';
import CreateProject from './pages/CreateProject';
import AccountLayout from './pages/AccountLayout';
import Account from './pages/Account';
import PlainLayout from './pages/PlainLayout';

const queryClient = new QueryClient();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <TooltipProvider delayDuration={800}>
      <QueryClientProvider client={queryClient}>
        <AuthenticationProvider>
          <BrowserRouter>
            <Routes>
              <Route element={<AppLayout />}>
                <Route path="/" element={<Home />} />
                <Route path="/project/create" element={<CreateProject />} />
                <Route path='/account' element={<AccountLayout />}>
                  <Route index element={<Account />} />
                </Route>
              </Route>
              <Route element={<PlainLayout />}>
                <Route path='/account/sign-up' element={<Signup />} />
                <Route path='/account/login' element={<Login />} />
              </Route>
            </Routes>
          </BrowserRouter>
        </AuthenticationProvider>
      </QueryClientProvider>
    </TooltipProvider>
    <Toaster richColors closeButton />
  </StrictMode>
);