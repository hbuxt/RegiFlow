import AppSidebarNavigation from "@/components/AppSidebarNavigation";
import AppSidebarTrigger from "@/components/AppSidebarTrigger";
import HeaderNavigation from "@/components/HeaderNavigation";
import { SidebarInset, SidebarProvider } from "@/components/ui/sidebar";
import { getSession } from "@/lib/utils/session";
import { Outlet, redirect } from "react-router";

export function appLayoutLoader() {
  const session = getSession();

  if (!session) {
    throw redirect("/account/login");
  }
}

export default function AppLayout() {
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