import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter, Route, Routes } from 'react-router';
import Home from './pages/Home';
import Signup from './pages/account/Signup';
import './main.css';
import AccountLayout from './pages/account/Layout';
import { AuthProvider } from './contexts/AuthContext';
import Login from './pages/account/Login';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route element={<AccountLayout />}>
            <Route path='/account/sign-up' element={<Signup />} />
            <Route path='/account/login' element={<Login />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  </StrictMode>
);