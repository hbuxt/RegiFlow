import { useAuthentication } from "@/hooks/useAuthentication";
import { Outlet } from "react-router";

export default function PlainLayout() {
  const { isAuthenticated } = useAuthentication();
  
  if (isAuthenticated) {
    window.location.href = "/";
    return;
  }

  return (
    <Outlet />
  );
}