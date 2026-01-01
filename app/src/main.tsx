import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Route, Routes } from 'react-router';
import Home from './pages/Home';
import Signup from './pages/Signup';
import './main.css';
import AuthLayout from './pages/AuthLayout';
import { AuthProvider } from './contexts/AuthContext';
import Login from './pages/Login';
import AppLayout from './pages/AppLayout';
import { TooltipProvider } from './components/ui/tooltip';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <TooltipProvider delayDuration={800}>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route element={<AppLayout />}>
              <Route path="/" element={<Home />} />
            </Route>
            <Route element={<AuthLayout />}>
              <Route path='/account/sign-up' element={<Signup />} />
              <Route path='/account/login' element={<Login />} />
            </Route>
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </TooltipProvider>
  </StrictMode>
);