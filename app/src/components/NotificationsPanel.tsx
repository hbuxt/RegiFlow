import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { getMyNotifications } from "@/lib/services/user";
import { Notification } from "@/lib/types/notification";
import { AppError } from "@/lib/utils/errors";
import Error from "@/pages/Error";
import { useQuery } from "@tanstack/react-query";
import { Item, ItemActions, ItemContent, ItemDescription, ItemGroup, ItemTitle } from "./ui/item";
import { Button } from "./ui/button";
import { Popover, PopoverContent, PopoverTrigger } from "./ui/popover";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { Bell } from "lucide-react";

export default function NotificationsPanel() {
  const { data, isPending, isError, error} = useQuery<Notification[], AppError>({
    queryKey: [QUERY_KEYS.GET_MY_NOTIFICATIONS],
    queryFn: getMyNotifications,
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });

  console.log(data);

  return (
    <Popover>
      <Tooltip delayDuration={400}>
        <TooltipTrigger asChild>
          <PopoverTrigger asChild>
            <Button className="rounded-full cursor-pointer" variant="ghost" size="icon">
              <Bell className="size-4" />
            </Button>
          </PopoverTrigger>
        </TooltipTrigger>
        <TooltipContent>Notifications</TooltipContent>
      </Tooltip>
      <PopoverContent className="w-full" side="bottom" align="end">
        {isPending ? (
          <>Loading...</>
        ) : (
          <>
            {isError ? (
              <Error title={error.message} errors={error.details} />
            ) : (
              <>
                {data.map((notification) => {
                  return (
                    <Item key={notification.id}>
                      <ItemContent>
                        <ItemTitle>Default Variant</ItemTitle>
                        <ItemDescription>
                          Standard styling with subtle background and borders.
                        </ItemDescription>
                      </ItemContent>
                      <ItemActions>
                        <Button variant="outline" size="sm">
                          Open
                        </Button>
                      </ItemActions>
                    </Item>
                  );
                })}
              </>
            )}
          </>
        )}
      </PopoverContent>
    </Popover>
  );
}