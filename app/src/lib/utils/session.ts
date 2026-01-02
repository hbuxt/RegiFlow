import { jwtDecode } from "jwt-decode";
import { Session, Token } from "../types/auth";

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