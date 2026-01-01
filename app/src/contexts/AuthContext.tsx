import { deleteSession, getSession, Session, setSession } from "@/lib/utils/session";
import { createContext, ReactNode, useContext, useState } from "react";

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
  const [sessionState, setSessionState] = useState<Session | null>(getSession());

  const authenticate = (accessToken: string) => {
    const session = setSession(accessToken);
    setSessionState(session);
  };

  const deauthenticate = () => {
    setSessionState(null);
    deleteSession();
  };

  return (
    <AuthContext.Provider value={{ session: sessionState, isAuthenticated: !!sessionState, authenticate, deauthenticate }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);