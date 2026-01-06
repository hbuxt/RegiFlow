import { getMyProjects, sortProjects } from "@/lib/services/project";
import { ArrowDownAZ, ArrowDownZA, ArrowUpDown, CalendarArrowDown, CalendarArrowUp, Check, CircleMinus, FolderCode, House, Layers2, Plus } from "lucide-react";
import { JSX, useEffect, useMemo, useState } from "react";
import { NavLink, useLocation } from "react-router";
import { Sidebar, SidebarContent, SidebarFooter, SidebarGroup, SidebarGroupContent, SidebarGroupLabel, SidebarHeader, SidebarMenu, SidebarMenuButton, SidebarMenuItem, SidebarRail } from "./ui/sidebar";
import { Button } from "./ui/button";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "./ui/dropdown-menu";
import { isRouteActive } from "@/lib/utils/route";
import { ellipsize } from "@/lib/utils/strings";
import { SORT_BY_AZ, SORT_BY_MOST_RECENT, SORT_BY_OLDEST, SORT_BY_ZA, SortBy } from "@/lib/constants/sort";
import { Empty, EmptyContent, EmptyDescription, EmptyHeader, EmptyMedia, EmptyTitle } from "./ui/empty";
import { Skeleton } from "./ui/skeleton";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { toast } from "sonner";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { useQuery } from "@tanstack/react-query";
import { Project } from "@/lib/types/project";
import { ApiError } from "@/lib/utils/result";
import { getMyPermissions } from "@/lib/services/user";

const generalLinks = [
  { name: "Home", url: "/", icon: <House /> }
];

const sortByOptions: { name: SortBy; icon: JSX.Element }[] = [
  { name: SORT_BY_AZ, icon: <ArrowDownAZ /> },
  { name: SORT_BY_ZA, icon: <ArrowDownZA /> },
  { name: SORT_BY_MOST_RECENT, icon: <CalendarArrowDown /> },
  { name: SORT_BY_OLDEST, icon: <CalendarArrowUp /> }
];

export default function AppSidebarNavigation() {
  const { data: permissions, isPending: isPermissionsPending, error: permissionsError } = useQuery<string[], ApiError>({
    queryKey: [QUERY_KEYS.GET_MY_PERMISSIONS],
    queryFn: getMyPermissions,
    staleTime: 1000 * 60 * 5,
    refetchOnWindowFocus: false,
    retry: false,
  });

  const { data, isPending: isMyProjectsPending, error: myProjectsError } = useQuery<Project[], ApiError>({
    queryKey: [QUERY_KEYS.GET_MY_PROJECTS],
    queryFn: getMyProjects,
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });
  
  const [sortBy, setSortBy] = useState<SortBy>(SORT_BY_AZ);
  const location = useLocation();

  useEffect(() => {
    if (myProjectsError) {
      toast.error("Failed to fetch your projects", {
        description: myProjectsError.errors?.map(e => e.message).join(", ") ?? "",
        duration: Infinity
      });
    }

    if (permissionsError) {
      toast.error("Failed to fetch your permissions", {
        description: permissionsError.errors?.map(e => e.message).join(", ") ?? "",
        duration: Infinity
      });
    }
  }, [myProjectsError, permissionsError]);

  const projects = useMemo(() => {
    if (!data) {
      return [];
    }

    return sortProjects(data, sortBy);
  }, [data, sortBy]);

  return (
    <Sidebar collapsible="offcanvas">
      <SidebarHeader className="border-sidebar-border h-16 border-b">
        <div className="flex h-full items-center gap-2 font-medium">
          <div className="bg-primary text-primary-foreground flex size-6 items-center justify-center rounded-md">
            <Layers2 className="size-4" />
          </div>
          RegiFlow
        </div>
      </SidebarHeader>
      <SidebarContent>
        <SidebarGroup className="flex gap-2 py-3">
          <SidebarGroupContent className="flex gap-2 overflow-visible">
            {isPermissionsPending || permissionsError ? (
              <Skeleton className="h-9 w-full rounded-md" />
            ) : (
              <>
                {permissions.includes(PERMISSIONS.PROJECT_CREATE) ? (
                  <Button asChild variant="outline" className="flex-1">
                    <NavLink to="/project/create">
                      <Plus />
                      <span>Project</span>
                    </NavLink>
                  </Button>
                ) : (
                  <Tooltip delayDuration={400}>
                    <TooltipTrigger asChild>
                      <span className="inline-block w-full">
                        <Button className="cursor-not-allowed w-full" variant="outline" disabled>
                          <Plus />
                          <span>Project</span>
                        </Button>
                      </span>
                    </TooltipTrigger>
                    <TooltipContent>
                      You don&apos;t have permission to create projects.
                    </TooltipContent>
                  </Tooltip>
                )}
              </>
            )}
            <Tooltip delayDuration={800}>
              <DropdownMenu>
                  <DropdownMenuTrigger asChild className="rounded-md">
                    <TooltipTrigger asChild>
                      <Button variant="outline" size="icon" className="flex-shrink-0 cursor-pointer">
                        <ArrowUpDown />
                      </Button>
                    </TooltipTrigger>
                  </DropdownMenuTrigger>
                <DropdownMenuContent>
                  <DropdownMenuGroup>
                    {sortByOptions.map((item) => (
                      <DropdownMenuItem key={item.name} className="cursor-pointer" onClick={() => setSortBy(item.name)}>
                        {item.icon}
                        {item.name}
                        {sortBy == item.name ? <Check className="ml-auto" /> : null}
                      </DropdownMenuItem>
                    ))}
                  </DropdownMenuGroup>
                </DropdownMenuContent>
              </DropdownMenu>
              <TooltipContent>
                <span>Sort</span>
              </TooltipContent>
            </Tooltip>
          </SidebarGroupContent>
        </SidebarGroup>
        <SidebarGroup>
          <SidebarGroupLabel>General</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {generalLinks.map((item) => (
                <SidebarMenuItem key={item.name}>
                  <SidebarMenuButton asChild isActive={isRouteActive(location.pathname, item.url)} className="cursor-pointer">
                      <NavLink to={item.url}>
                        {item.icon}
                        <span>{item.name}</span>
                      </NavLink>
                    </SidebarMenuButton>
                </SidebarMenuItem>
              ))}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
        <SidebarGroup>
          <SidebarGroupLabel>Projects</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {isMyProjectsPending || myProjectsError || isPermissionsPending || permissionsError ? (
                <>
                  <Skeleton className="h-8 w-full rounded-md" />
                  <Skeleton className="h-8 w-2/3 rounded-md" />
                  <Skeleton className="h-8 w-3/4 rounded-md" />
                  <Skeleton className="h-8 w-1/2 rounded-md" />
                  <Skeleton className="h-8 w-full rounded-md" />
                  <Skeleton className="h-8 w-3/4 rounded-md" />
                </>
              ) : (
                <>
                  {permissions.includes(PERMISSIONS.PROJECT_READ) ? (
                    <>
                      {!projects || projects.length == 0 ? (
                        <Empty className="px-0 md:px-0">
                          <EmptyHeader>
                            <EmptyMedia variant="icon">
                              <FolderCode />
                            </EmptyMedia>
                            <EmptyTitle>No Projects Yet</EmptyTitle>
                            <EmptyDescription>
                              You haven&apos;t created any projects yet. Get started by creating
                              your first project.
                            </EmptyDescription>
                          </EmptyHeader>
                          <EmptyContent>
                            <div className="flex">
                              <Button variant="default" type="button" asChild>
                                <NavLink to="/project/create">Create project</NavLink>
                              </Button>
                            </div>
                          </EmptyContent>
                        </Empty>
                      ) : (
                        <>
                          {projects.map((item) => (
                            <SidebarMenuItem key={item.id}>
                              <Tooltip delayDuration={800}>
                                <TooltipTrigger asChild>
                                  <SidebarMenuButton asChild isActive={isRouteActive(location.pathname, item.name)} className="cursor-pointer truncate">
                                    <NavLink to={`/${item.id}`}>{ellipsize(item.name, 25)}</NavLink>
                                  </SidebarMenuButton>
                                </TooltipTrigger>
                                <TooltipContent>
                                  {item.name}
                                  {item.createdAt ? <p>Created on {item.createdAt.toDateString()}</p> : null}
                                </TooltipContent>
                              </Tooltip>
                            </SidebarMenuItem>
                          ))}
                        </>
                      )}
                    </>
                  ) : (
                    <Empty className="px-0 md:px-0">
                      <EmptyHeader>
                        <EmptyMedia variant="icon">
                          <CircleMinus />
                        </EmptyMedia>
                        <EmptyTitle>No Permission</EmptyTitle>
                        <EmptyDescription>
                          You don&apos;t have permission to view projects.
                        </EmptyDescription>
                      </EmptyHeader>
                    </Empty>
                  )}
                </>
              )}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
      <SidebarFooter className="border-sidebar-border border-t">
        <SidebarMenu>
          <SidebarMenuItem className="flex justify-center items-center text-sm text-sidebar-foreground/70">
            <span>v1.0.0-alpha</span>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  );
}