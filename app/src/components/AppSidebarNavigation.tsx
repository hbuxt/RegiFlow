import { SortBy, sortByAz, sortByMostRecent, sortByOldest, sortByZa } from "@/lib/types/common";
import { sortProjects } from "@/lib/services/project";
import { ArrowDownAZ, ArrowDownZA, ArrowUpDown, CalendarArrowDown, CalendarArrowUp, Check, House, Layers2, Plus } from "lucide-react";
import { useState } from "react";
import { NavLink, useLocation } from "react-router";
import { Sidebar, SidebarContent, SidebarFooter, SidebarGroup, SidebarGroupContent, SidebarGroupLabel, SidebarHeader, SidebarMenu, SidebarMenuButton, SidebarMenuItem, SidebarRail } from "./ui/sidebar";
import { Button } from "./ui/button";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "./ui/dropdown-menu";
import { isRouteActive } from "@/lib/utils/route";
import { ellipsizable, ellipsize } from "@/lib/utils/strings";

const generalLinks = [
  { name: "Home", url: "/", icon: <House /> }
];

const data = [
  { id: "123", name: "Royal Academy of Engineering", createdAt: new Date(2021, 6, 14) },
  { id: "456", name: "Geological Society", createdAt: new Date(2024, 10, 3) },
  { id: "789", name: "RegiFlow", createdAt: new Date(2025, 12, 25) },
  { id: "101112", name: "Hastions", createdAt: new Date(2025, 7, 16) },
  { id: "131415", name: "Royal Osteoporosis Society", createdAt: new Date(2025, 1, 12) }
];

const sortByOptions = [
  { name: sortByAz, icon: <ArrowDownAZ /> },
  { name: sortByZa, icon: <ArrowDownZA /> },
  { name: sortByMostRecent, icon: <CalendarArrowDown /> },
  { name: sortByOldest, icon: <CalendarArrowUp /> }
];

export default function AppSidebarNavigation() {
  const [sortBy, setSortBy] = useState<SortBy>(sortByAz);
  const location = useLocation();

  const projects = sortProjects(data, sortBy);

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
            <Button asChild variant="outline" className="flex-1">
              <NavLink to="/">
                <Plus />
                <span>Project</span>
              </NavLink>
            </Button>
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
              {projects.map((item) => (
                <SidebarMenuItem key={item.id}>
                  <Tooltip delayDuration={800}>
                    <TooltipTrigger asChild>
                      <SidebarMenuButton asChild isActive={isRouteActive(location.pathname, item.name)} className="cursor-pointer">
                        <NavLink to={`/${item.name}`}>{ellipsize(item.name, 28)}</NavLink>
                      </SidebarMenuButton>
                    </TooltipTrigger>
                    <TooltipContent>
                      {ellipsizable(item.name, 28) ? <p>{ellipsize(item.name, 28)}</p> : null}
                      {item.createdAt ? <p>Created on {item.createdAt.toDateString()}</p> : null}
                    </TooltipContent>
                  </Tooltip>
                </SidebarMenuItem>
              ))}
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