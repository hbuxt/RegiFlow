import { AuthenticationContext } from "@/contexts/AuthenticationContext";
import { useContext } from "react";

export function useAuthentication() {
  const ctx = useContext(AuthenticationContext);

  if (!ctx) {
    throw new Error("useAuthentication must be used within an AuthenticationProvider");
  }

  return ctx;
}