import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { getMyDetails, getMyPermissions } from "@/lib/services/user";
import { User } from "@/lib/types/user";
import { isRouteActive } from "@/lib/utils/route";
import { cn } from "@/lib/utils/styles";
import queryClient from "@/lib/utils/tanstack";
import { useQuery } from "@tanstack/react-query";
import { Settings } from "lucide-react";
import { isRouteErrorResponse, NavLink, Outlet, redirect, useLoaderData, useLocation, useRouteError } from "react-router";
import Error from "./Error";
import { AppError } from "@/lib/utils/errors";
import { ReactNode } from "react";

export async function accountLayoutLoader() {
  try {
    const user = await queryClient.fetchQuery<User, AppError>({
      queryKey: [QUERY_KEYS.GET_MY_DETAILS],
      queryFn: getMyDetails,
      staleTime: 1000 * 60 * 3,
      retry: false
    });

    const permissions = await queryClient.fetchQuery<string[], AppError>({
      queryKey: [QUERY_KEYS.GET_MY_PERMISSIONS],
      queryFn: getMyPermissions,
      staleTime: 1000 * 60 * 3,
      retry: false
    });

    return { initUser: user, initPermissions: permissions };
  } catch (e) {
    if (e instanceof AppError) {
      if (e.status == 401) {
        throw redirect("/account/login");
      }

      throw new Response(JSON.stringify(e.details ?? []), {
        status: e.status,
        statusText: e.message,
        headers: { "Content-Type": "application/json" }
      });
    }

    throw e;
  }
}

export function AccountLayoutError(): ReactNode {
  const error = useRouteError();

  if (isRouteErrorResponse(error)) {
    return <Error title="Unable to load your account" errors={error.data ?? []} />
  }

  throw error;
}

export default function AccountLayout() {
  const location = useLocation();
  const { initUser, initPermissions } = useLoaderData() as { initUser: User, initPermissions: string[] };
  const { data: user } = useQuery<User, AppError>({
    queryKey: [QUERY_KEYS.GET_MY_DETAILS],
    queryFn: getMyDetails,
    initialData: initUser,
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });

  const links = [
    { name: "Account", url: "/account", icon: <Settings size={16} /> }
  ];

  return (
    <>
      <div className="px-8 pt-8">
        <div className="flex items-center gap-2 text-left text-sm">
          <Avatar className="h-16 w-16 rounded-full">
            <AvatarFallback className="rounded-full bg-cyan-500 text-white text-xl">{user.email[0].toUpperCase()}</AvatarFallback>
          </Avatar>
          <div className="grid flex-1 text-left text-sm leading-tight">
            <span className="truncate font-medium text-lg">{user.email}</span>
            <span className="truncate text-md text-muted-foreground">Your account details</span>
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
          <Outlet context={{ user, permissions: initPermissions }} />
        </main>
      </div>
    </>
  );
}