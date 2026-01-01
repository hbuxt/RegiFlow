import { useAuth } from "@/contexts/AuthContext";
import { Outlet } from "react-router";

export default function AuthLayout() {
  const { isAuthenticated } = useAuth();
  
  if (isAuthenticated) {
    window.location.href = "/";
    return;
  }

  return (
    <Outlet />
  );
}