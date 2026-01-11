import { Project } from "@/lib/types/project";
import { isExactRouteActive, isRouteActive } from "@/lib/utils/route";
import { cn } from "@/lib/utils/styles";
import { Settings, Users } from "lucide-react";
import { NavLink, Outlet, useLocation, useOutletContext, useParams } from "react-router";

export default function ProjectSettingsLayout() {
  const location = useLocation();
  const { project, permissions } = useOutletContext() as { project: Project, permissions: string[] };
  
  const links = [
    { name: "General", url: `/${project.id}/settings`, icon: <Settings size={16} /> },
    { name: "Access", url: `/${project.id}/settings/access`, icon: <Users size={16} /> }
  ];

  return (
    <div className="flex flex-col md:flex-row gap-4 p-8">
      <aside className="w-full md:w-[240px]">
        <nav>
          <ul className="w-full flex flex-col gap-1">
            {links.map((item) => (
              <li key={item.name}>
                <NavLink to={item.url} className={cn("w-full flex items-center gap-2 py-1.5 px-2 rounded-md font-normal text-sm hover:bg-sidebar-accent hover:text-sidebar-accent-foreground", isExactRouteActive(location.pathname, item.url) ? "bg-sidebar-accent text-sidebar-accent-foreground font-medium" : "")}>
                  {item.icon}
                  <span>{item.name}</span>
                </NavLink>
              </li>
            ))}
          </ul>
        </nav>
      </aside>
      <main className="flex-1">
        <Outlet context={{ project, permissions }} />
      </main>
    </div>
  );
}