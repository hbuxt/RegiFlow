import { NavLink } from "react-router";
import { Button } from "./ui/button";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { Bell, Loader, LogOut, Settings } from "lucide-react";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "./ui/dropdown-menu";
import { Avatar, AvatarFallback } from "./ui/avatar";
import { useEffect, useState } from "react";
import { Skeleton } from "./ui/skeleton";
import { toast } from "sonner";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useAuthentication } from "@/hooks/useAuthentication";
import { User } from "@/lib/types/user";
import { ApiError } from "@/lib/utils/result";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { getMyDetails } from "@/lib/services/user";

export default function HeaderNavigation() {
  const { data, isPending, error } = useQuery<User, ApiError>({
    queryKey: [QUERY_KEYS.GET_MY_DETAILS],
    queryFn: getMyDetails,
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });

  const queryClient = useQueryClient();
  const { deauthenticate } = useAuthentication();
  const [loggingOut, setLoggingOut] = useState(false);

  function onLogout() {
    setLoggingOut(true);
    deauthenticate();
    queryClient.clear();
    window.location.href = "/";
    return;
  }

  useEffect(() => {
    if (!error) {
      return;
    }

    toast.error("Failed to fetch your details", {
      description: error.errors?.map(e => e.message).join(", ") ?? "",
      duration: Infinity
    });

  }, [error]);

  return (
    <div className="ml-auto px-6">
      <div className="flex items-center gap-3">
        <Tooltip delayDuration={400}>
          <TooltipTrigger asChild>
            <Button className="rounded-full cursor-pointer" variant="ghost" size="icon">
              <NavLink to="/">
                <Bell className="size-4" />
              </NavLink>
            </Button>
          </TooltipTrigger>
          <TooltipContent>
            Notifications
          </TooltipContent>
        </Tooltip>
        <DropdownMenu>
          <DropdownMenuTrigger asChild className="cursor-pointer rounded-full">
            <Avatar className="h-8 w-8 rounded-full">
              {isPending || error ? (
                <Skeleton className="h-12 w-12 rounded-full" />
              ) : (
                <AvatarFallback className="rounded-full bg-cyan-500 text-white">
                  {data.email[0].toUpperCase()}
                </AvatarFallback>
              )}
            </Avatar>
          </DropdownMenuTrigger>
            <DropdownMenuContent className="w-(--radix-dropdown-menu-trigger-width) min-w-56 rounded-lg" side="bottom" align="end" sideOffset={4}>
              <DropdownMenuLabel className="p-0 font-normal">
                <div className="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
                  <Avatar className="h-8 w-8 rounded-full">
                    {isPending || error ? (
                      <Skeleton className="h-12 w-12 rounded-full" />
                    ) : (
                      <AvatarFallback className="rounded-full bg-cyan-500 text-white">{data.email[0].toUpperCase()}</AvatarFallback>
                    )}
                  </Avatar>
                  <div className="grid flex-1 text-left text-sm leading-tight">
                    {isPending || error ? (
                        <div className="flex flex-col gap-1">
                          <Skeleton className="h-4 w-[25%]" />
                          <Skeleton className="h-4 w-[80%]" />
                        </div>
                    ) : (
                      <>
                        <span className="truncate font-medium">Me</span>
                        <span className="truncate text-xs">{data.email}</span>
                      </>
                    )}
                  </div>
                </div>
              </DropdownMenuLabel>
              <DropdownMenuSeparator />
              <DropdownMenuGroup>
                {isPending || error ? (
                  <div className="flex flex-row gap-1 px-1 items-center">
                    <Skeleton className="h-8 w-8 rounded-full" />
                    <Skeleton className="h-6 w-[50%]" />
                  </div>
                ) : (
                  <DropdownMenuItem asChild className="cursor-pointer">
                    <NavLink to="/account">
                      <Settings />
                      Account
                    </NavLink>
                  </DropdownMenuItem>
                )}
              </DropdownMenuGroup>
              <DropdownMenuSeparator />
              <DropdownMenuGroup>
                {isPending || error ? (
                  <div className="flex flex-row gap-1 px-1 items-center">
                    <Skeleton className="h-8 w-8 rounded-full" />
                    <Skeleton className="h-6 w-[35%]" />
                  </div>
                ) : (
                  <DropdownMenuItem className="cursor-pointer" onSelect={onLogout}>
                    {loggingOut ? (
                      <><Loader className="animate-spin" /><span>Logging out</span></>
                    ) : (
                      <><LogOut /><span>Log out</span></>
                    )}
                  </DropdownMenuItem>
                )}
              </DropdownMenuGroup>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </div>
  );
}