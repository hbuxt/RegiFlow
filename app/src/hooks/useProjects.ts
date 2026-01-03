import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { useAuthentication } from "./useAuthentication";
import { useQuery } from "@tanstack/react-query";
import { getMyProjects } from "@/lib/services/project";
import { ValueResult } from "@/lib/utils/result";
import { Project } from "@/lib/types/project";

export function useMyProjects() {
  const { isAuthenticated } = useAuthentication();

  return useQuery<ValueResult<Project[]>, unknown>({
    queryKey: [QUERY_KEYS.GET_MY_PROJECTS],
    queryFn: getMyProjects,
    enabled: isAuthenticated,
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });
}