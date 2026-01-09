import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { getProjectById } from "@/lib/services/project";
import { Project } from "@/lib/types/project";
import { AppError } from "@/lib/utils/errors";
import queryClient from "@/lib/utils/tanstack";
import { isRouteErrorResponse, LoaderFunctionArgs, NavLink, Outlet, redirect, useLoaderData, useLocation, useParams, useRouteError } from "react-router";
import Error from "./Error";
import { useQuery } from "@tanstack/react-query";
import { Gauge, Settings, Tickets } from "lucide-react";
import { cn } from "@/lib/utils/styles";
import { isExactRouteActive } from "@/lib/utils/route";
import { ReactNode } from "react";

export async function projectLayoutLoader({ params }: LoaderFunctionArgs) {
  try {
    const project = await queryClient.fetchQuery<Project, AppError>({
      queryKey: [QUERY_KEYS.GET_PROJECT_BY_ID(params.id!)],
      queryFn: () => getProjectById(params.id!),
      staleTime: 1000 * 60 * 3,
      retry: false
    });

    return { initProject: project };
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

export function ProjectLayoutError(): ReactNode {
  const error = useRouteError();

  if (isRouteErrorResponse(error)) {
    <Error title="Unable to load project" errors={error.data ?? []} />
  }

  throw error;
}

export default function ProjectLayout() {
  const location = useLocation();
  const { id } = useParams<{ id: string }>();
  const { initProject } = useLoaderData() as { initProject: Project };
  const { data } = useQuery<Project, AppError>({
    queryKey: [QUERY_KEYS.GET_PROJECT_BY_ID(id!)],
    queryFn: () => getProjectById(id!),
    initialData: initProject,
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });

  const links = [
    { name: "Overview", url: `/${data.id}`, icon: <Gauge size={18} /> },
    { name: "Tickets", url: `/${data.id}/tickets`, icon: <Tickets size={18} /> },
    { name: "Settings", url: `/${data.id}/settings`, icon: <Settings size={18} /> }
  ];

  return (
    <>
      <div className="bg-slate-50 pt-8">
        <div className="px-8 flex items-center gap-3">
          <div className="text-start flex flex-col gap-2 w-full max-w-full sm:max-w-[80%] lg:max-w-[60%]">
            <h1 className="text-xl font-semibold">{data.name}</h1>
            <p className="text-sm text-muted-foreground">{data.description}</p>
          </div>
        </div>
        <nav className="w-full pt-6 border-b">
          <ul className="w-full px-8 bg-slate-50 flex flex-col md:flex-row">
            {links.map((item) => (
              <li key={item.name}>
                <NavLink to={item.url} className={cn("flex items-center gap-2 py-2 px-4 border-b-2 font-normal text-sm hover:text-primary", isExactRouteActive(location.pathname, item.url) ? "border-primary text-primary" : "border-transparent text-muted-foreground")}>
                  {item.icon}
                  <span>{item.name}</span>
                </NavLink>
              </li>
            ))}
          </ul>
        </nav>
      </div>
      <Outlet />
    </>
  );
}