import { jwtDecode } from "jwt-decode";

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

export function getSession(): Session | null {
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
}

export function setSession(accessToken: string): Session {
  localStorage.setItem("RegiContext", accessToken);
  const data = jwtDecode<Token>(accessToken);
  
  return {
    id: data.sub,
    token: data,
    rawToken: accessToken
  };
}

export function deleteSession() {
  localStorage.removeItem("RegiContext");
}