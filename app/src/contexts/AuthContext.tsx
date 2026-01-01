import { jwtDecode } from "jwt-decode";
import { createContext, ReactNode, useContext, useEffect, useState } from "react";

export interface Session {
  id: string;
  token: Token;
  rawToken: string;
}

export interface Token {
  sub: string;
  aud: string;
  iss: string;
  exp: number;
  iat: number;
  nbf: number;
}

interface AuthContextType {
  session: Session | null,
  isAuthenticated: boolean;
  authenticate: (accessToken: string) => void;
  deauthenticate: () => void;
}

const AuthContext = createContext<AuthContextType>({
  session: null,
  isAuthenticated: false,
  authenticate: () => {},
  deauthenticate: () => {}
});

export function AuthProvider({ children }: { children: ReactNode }) {
  const [session, setSession] = useState<Session | null>(() => {
    const accessToken = localStorage.getItem("RegiContext");

    if (!accessToken) {
      return null;
    }

    const data = jwtDecode<Token>(accessToken);

    if (!data) {
      return null;
    }

    return {
      id: data.sub,
      token: data,
      rawToken: accessToken
    };
  });

  const authenticate = (accessToken: string) => {
    const data = jwtDecode<Token>(accessToken);

    setSession({
      id: data.sub,
      token: data,
      rawToken: accessToken
    });
    localStorage.setItem("RegiContext", accessToken);
  };

  const deauthenticate = () => {
    setSession(null);
    localStorage.removeItem("RegiContext");
  };

  return (
    <AuthContext.Provider value={{ session, isAuthenticated: !!session, authenticate, deauthenticate }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);