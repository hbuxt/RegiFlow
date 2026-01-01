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
  login: (accessToken: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType>({
  session: null,
  isAuthenticated: false,
  login: () => {},
  logout: () => {}
});

export function AuthProvider({ children }: { children: ReactNode }) {
  const [session, setSession] = useState<Session | null>(null);

  useEffect(() => {
    const accessToken = localStorage.getItem("RegiContext");
    
    if (accessToken) {
      const data = jwtDecode<Token>(accessToken);

      setSession({
        id: data.sub,
        token: data,
        rawToken: accessToken
      });
    }
    
  }, []);

  const login = (accessToken: string) => {
    const data = jwtDecode<Token>(accessToken);

    setSession({
      id: data.sub,
      token: data,
      rawToken: accessToken
    });
    localStorage.setItem("RegiContext", accessToken);
  };

  const logout = () => {
    setSession(null);
    localStorage.removeItem("RegiContext");
  };

  return (
    <AuthContext.Provider value={{ session, isAuthenticated: !!session, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);