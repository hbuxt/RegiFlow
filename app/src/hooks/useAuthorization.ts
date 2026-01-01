import { getMyPermissions } from "@/lib/services/user";
import { ValueResult } from "@/lib/utils/result";
import { useQuery } from "@tanstack/react-query";

export function useAuthorization() {
  const { data, isLoading, isError } = useQuery<ValueResult<string[]>>({
    queryKey: ["myPermissions"],
    queryFn: getMyPermissions,
    staleTime: 1000 * 60 * 5,
    retry: false,
  });

  const hasPermission = (permission: string) => {
    return data?.value?.includes(permission) ?? false;
  }

  return {
    hasPermission,
    isLoading,
    isError
  };
}