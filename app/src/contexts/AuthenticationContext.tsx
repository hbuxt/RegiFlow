import { Session } from "@/lib/types/auth";
import { deleteSession, getSession, setSession } from "@/lib/utils/session";
import { createContext, ReactNode, useCallback, useMemo, useState } from "react";

interface AuthenticationContextType {
  session: Session | null,
  isAuthenticated: boolean;
  authenticate: (accessToken: string) => void;
  deauthenticate: () => void;
}

export const AuthenticationContext = createContext<AuthenticationContextType | null>(null);

export function AuthenticationProvider({ children }: { children: ReactNode }) {
  const [sessionState, setSessionState] = useState<Session | null>(() => getSession());

  const authenticate = useCallback((accessToken: string) => {
    const session = setSession(accessToken);
    setSessionState(session);
  }, []);

  const deauthenticate = useCallback(() => {
    deleteSession();
    setSessionState(null);
  }, []);

  const value = useMemo(() => ({
    session: sessionState,
    isAuthenticated: !!sessionState,
    authenticate,
    deauthenticate,
  }), [sessionState, authenticate, deauthenticate]);

  return <AuthenticationContext.Provider value={value}>{children}</AuthenticationContext.Provider>;
}