import { getMyPermissions } from "@/lib/services/user";
import { useQuery } from "@tanstack/react-query";
import { useAuthentication } from "./useAuthentication";
import { useCallback } from "react";
import { ValueResult } from "@/lib/utils/result";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";

export function useAuthorization() {
  const { isAuthenticated } = useAuthentication();

  const query = useQuery<ValueResult<string[]>>({
    queryKey: [QUERY_KEYS.GET_MY_PERMISSIONS],
    queryFn: getMyPermissions,
    enabled: isAuthenticated,
    staleTime: 1000 * 60 * 5,
    retry: false,
  });

  const hasPermission = useCallback((permission: string) =>
    query.data?.value?.includes(permission) ?? false, 
    [query.data]
  );

  return {
    hasPermission,
    permissions: query.data?.value ?? [],
    isLoading: query.isLoading,
    isError: query.isError,
  };
}