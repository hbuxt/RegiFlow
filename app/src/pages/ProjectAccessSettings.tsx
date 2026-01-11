import DataTable from "@/components/DataTable";
import { Empty, EmptyDescription, EmptyHeader, EmptyMedia, EmptyTitle } from "@/components/ui/empty";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { getUsersInProject } from "@/lib/services/project";
import { Project, ProjectUser, ProjectUserRole } from "@/lib/types/project";
import { AppError } from "@/lib/utils/errors";
import { useQuery } from "@tanstack/react-query";
import { ColumnDef } from "@tanstack/react-table";
import { Ban } from "lucide-react";
import { useOutletContext } from "react-router";
import Error from "./Error";
import { Skeleton } from "@/components/ui/skeleton";
import { InviteUserToProjectForm } from "@/components/InviteUserToProjectForm";

export default function ProjectAccessSettings() {
  const { project, permissions } = useOutletContext() as { project: Project, permissions: string[] };

  return (
    <div className="flex flex-col gap-4">
      <div>
        <div className="w-full border-b pb-4">
          <h2 className="text-lg font-normal">Access</h2>
        </div>
      </div>
      <div>
        <div className="flex items-center justify-between mb-4">
          <div className="">
            <h2 className="text-md mb-1">Users</h2>
            <p className="text-sm text-muted-foreground mb-0">A list of users in the project.</p>
          </div>
          <InviteUserToProjectForm project={project} permissions={permissions} />
        </div>
        {permissions.includes(PERMISSIONS.PROJECT_USERS_READ) ? (
          <ProjectUsersDataTable project={project} />
        ) : (
          <Empty className="border border-dashed">
            <EmptyHeader>
              <EmptyMedia variant="icon">
                <Ban />
              </EmptyMedia>
              <EmptyTitle>No permission</EmptyTitle>
              <EmptyDescription>
                You don&apos;t have permission to view users in this project.
              </EmptyDescription>
            </EmptyHeader>
          </Empty>
        )}
      </div>
    </div>
  );
}

function ProjectUsersDataTable({ project }: { project: Project }) {
  const { data: users, isPending, isError, error } = useQuery<ProjectUser[], AppError>({
    queryKey: [QUERY_KEYS.GET_USERS_IN_PROJECT, project.id],
    queryFn: () => getUsersInProject(project.id),
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });

  const columns: ColumnDef<ProjectUser>[] = [
    {
      accessorKey: "firstName",
      header: "First name",
      cell: ({ row }) => {
        const value = row.getValue<string | null>("firstName");

        if (!value) {
          return "-";
        }

        return value;
      }
    },
    {
      accessorKey: "lastName",
      header: "Last name",
      cell: ({ row }) => {
        const value = row.getValue<string | null>("lastName");

        if (!value) {
          return "-";
        }

        return value;
      }
    },
    {
      accessorKey: "email",
      header: "Email",
      cell: ({ row }) => {
        const value = row.getValue<string | null>("email");

        if (!value) {
          return "-";
        }

        return value;
      }
    },
    {
      accessorKey: "roles",
      header: "Role",
      cell: ({ row }) => {
        const roles = row.getValue<ProjectUserRole[]>("roles");

        if (!roles || roles.length == 0) {
          return "-"
        }

        return roles.map(role => role.name).join(", ");
      }
    },
    {
      accessorKey: "joinedAt",
      header: "Joined",
      cell: ({ row }) => {
        const value = row.getValue<string | Date>("joinedAt");
        const date = value instanceof Date ? value : new Date(value);

        if (isNaN(date.getTime())) {
          return "—";
        }

        return date.toLocaleDateString(undefined, {
          year: "numeric",
          month: "short",
          day: "numeric",
        });
      }
    }
  ];

  if (isPending) {
    const rows = 5;

    return (
      <div className="rounded-md border">
        <div className="grid grid-cols-5 gap-4 border-b px-4 py-3">
          <Skeleton className="h-5 w-full" />
          <Skeleton className="h-5 w-full" />
          <Skeleton className="h-5 w-full" />
          <Skeleton className="h-5 w-full" />
          <Skeleton className="h-5 w-full" />
        </div>
        {Array.from({ length: rows }).map((_, rowIndex) => (
          <div key={rowIndex} className="grid grid-cols-5 gap-4 px-4 py-3 border-b last:border-b-0">
            <Skeleton className="h-5 w-full" />
            <Skeleton className="h-5 w-full" />
            <Skeleton className="h-5 w-full" />
            <Skeleton className="h-5 w-full" />
            <Skeleton className="h-5 w-full" />
          </div>
        ))}
      </div>
    );
  }

  if (isError) {
    return <Error title={error.message} errors={error.details} />;
  }

  return <DataTable columns={columns} data={users} />;
}