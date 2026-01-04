import CreateProjectForm from "@/components/CreateProjectForm";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { getMyPermissions } from "@/lib/services/user";
import { HttpClientError } from "@/lib/utils/http";
import { ApiError } from "@/lib/utils/result";
import queryClient from "@/lib/utils/tanstack";
import { isRouteErrorResponse, redirect, useRouteError } from "react-router";
import Error from "./Error";

export async function createProjectLoader() {
  try {
    const permissions = await queryClient.fetchQuery<string[], ApiError>({
      queryKey: [QUERY_KEYS.GET_MY_PERMISSIONS],
      queryFn: getMyPermissions,
      staleTime: 1000 * 60 * 3,
      retry: false
    });

    return permissions;
  } catch (e) {
    if (e instanceof HttpClientError) {
      if (e.status == 401) {
        throw redirect("/account/login");
      }

      throw new Response(JSON.stringify(e.data ?? []), {
        status: e.status,
        statusText: e.message,
        headers: { "Content-Type": "application/json" }
      });
    }

    throw e;
  }
}

export function CreateProjectError() {
  const error = useRouteError();

  if (isRouteErrorResponse(error)) {
    const title = "Unable to load project creation page";

    switch (error.status) {
      case 403:
        return <Error title={title} errors={error.data ?? []} />;
      case 404:
        return <Error title={title} errors={error.data ?? []} />;
      case 503: 
        return <Error title={title} errors={error.data ?? []} />;
      default:
        return <Error title={title} errors={error.data ?? []} />;
    }
  }

  throw error;
}

export default function CreateProject() {
  return (
    <div className="bg-muted flex min-h-svh flex-col items-center justify-center gap-6 p-6 md:p-10">
      <div className="flex w-full max-w-sm flex-col">
        <CreateProjectForm />
      </div>
    </div>
  );
}