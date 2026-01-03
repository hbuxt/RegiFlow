import { getMyDetails } from "@/lib/services/user";
import { User } from "@/lib/types/user";
import { ApiError } from "@/lib/utils/result";
import { useQuery } from "@tanstack/react-query";
import { useAuthentication } from "./useAuthentication";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";

export function useMyDetails() {
  const { isAuthenticated } = useAuthentication();

  return useQuery<User, ApiError>({
    queryKey: [QUERY_KEYS.GET_MY_DETAILS],
    queryFn: getMyDetails,
    enabled: isAuthenticated,
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });
}