import AppSidebarNavigation from "@/components/AppSidebarNavigation";
import AppSidebarTrigger from "@/components/AppSidebarTrigger";
import HeaderNavigation from "@/components/HeaderNavigation";
import { SidebarInset, SidebarProvider } from "@/components/ui/sidebar";
import { useAuth } from "@/contexts/AuthContext";
import { Outlet } from "react-router";

export default function AppLayout() {
  const { isAuthenticated } = useAuth();
  
  if (!isAuthenticated) {
    window.location.href = "/account/login";
    return;
  }

  return (
    <SidebarProvider>
      <AppSidebarNavigation />
      <SidebarInset>
        <header className="flex h-16 shrink-0 items-center gap-2 border-b px-4">
          <AppSidebarTrigger />
          <HeaderNavigation />
        </header>
        <div className="flex-1">
          <Outlet />
        </div>
      </SidebarInset>
    </SidebarProvider>
  );
}