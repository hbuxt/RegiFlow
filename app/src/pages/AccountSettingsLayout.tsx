import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Skeleton } from "@/components/ui/skeleton";
import { useMyDetails } from "@/hooks/useUser";
import { isRouteActive } from "@/lib/utils/route";
import { cn } from "@/lib/utils/styles";
import { Settings } from "lucide-react";
import { NavLink, Outlet, useLocation } from "react-router";

const links = [
  { name: "Account", url: `/account/settings`, icon: <Settings size={16} /> }
]

export default function AccountSettingsLayout() {
  const { data, isLoading, isError } = useMyDetails();
  const location = useLocation();

  return (
    <>
      <div className="px-8 pt-8">
        <div className="flex items-center gap-2 text-left text-sm">
          <Avatar className="h-16 w-16 rounded-full">
            {isLoading || isError || data?.error ? (
              <Skeleton className="h-16 w-16 rounded-full" />
            ) : (
              <AvatarFallback className="rounded-full bg-cyan-500 text-white text-xl">{data?.value?.email?.[0].toUpperCase()}</AvatarFallback>
            )}
          </Avatar>
          <div className="grid flex-1 text-left text-sm leading-tight">
            {isLoading || isError || data?.error ? (
                <div className="flex flex-col gap-2">
                  <Skeleton className="h-6 w-[150px]" />
                  <Skeleton className="h-6 w-[125px]" />
                </div>
            ) : (
              <>
                <span className="truncate font-medium text-lg">{data?.value?.email}</span>
                <span className="truncate text-md text-muted-foreground">Your account details</span>
              </>
            )}
          </div>
        </div>
      </div>
      <div className="flex flex-col md:flex-row gap-4 p-8">
        <aside className="w-full md:w-[240px]">
          <nav>
            <ul className="w-full flex flex-col gap-1">
              {links.map((item) => (
                <li key={item.name}>
                  <NavLink to={item.url} className={cn("w-full flex items-center gap-2 py-1.5 px-2 rounded-md font-normal text-sm hover:bg-sidebar-accent hover:text-sidebar-accent-foreground", isRouteActive(location.pathname, item.url) ? "bg-sidebar-accent text-sidebar-accent-foreground font-medium" : "")}>
                    {item.icon}
                    <span>{item.name}</span>
                  </NavLink>
                </li>
              ))}
            </ul>
          </nav>
        </aside>
        <main className="flex-1">
          <Outlet />
        </main>
      </div>
    </>
  );
}