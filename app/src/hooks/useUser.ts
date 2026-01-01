import { getMyDetails } from "@/lib/services/user";
import { User } from "@/lib/types/user";
import { ValueResult } from "@/lib/utils/result";
import { useQuery } from "@tanstack/react-query";

export function useMyDetails() {
  return useQuery<ValueResult<User>, unknown>({
    queryKey: ["myDetails"],
    queryFn: getMyDetails,
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });
}