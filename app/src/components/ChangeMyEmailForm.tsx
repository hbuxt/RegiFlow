import { useAuthorization } from "@/hooks/useAuthorization";
import { useMyDetails } from "@/hooks/useUser";
import { Label } from "./ui/label";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { Skeleton } from "./ui/skeleton";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { useEffect } from "react";
import { toast } from "sonner";

export default function ChangeMyEmailForm() {
  const { data, isPending: isMyDetailsPending, error: myDetailsError } = useMyDetails();
  const { hasPermission, isPending: isPermissionsPending, error: permissionError } = useAuthorization();

  useEffect(() => {
    if (myDetailsError) {
      toast.error("Failed to fetch your details", {
        description: myDetailsError.errors?.map(e => e.message).join(", ") ?? "",
        duration: Infinity
      });
    }

    if (permissionError) {
      toast.error("Failed to fetch your permissions", {
        description: permissionError.errors?.map(e => e.message).join(", ") ?? "",
        duration: Infinity
      });
    }
  }, [myDetailsError, permissionError]);

  return (
    <div className="pb-3">
      <div className="flex flex-row gap-4 justify-between py-3">
        <div className="flex flex-col flex-1 gap-2">
          <Label htmlFor="email">Email</Label>
          <Input id="email" type="email" defaultValue={data?.email} disabled={isMyDetailsPending || !!myDetailsError || isPermissionsPending || !!permissionError || !hasPermission(PERMISSIONS.USER_EMAIL_UPDATE)} />
        </div>
        <div className="flex items-end md:justify-end">
          <div className="flex items-center justify-end">
              {isMyDetailsPending || myDetailsError || isPermissionsPending || permissionError ? (
                <Skeleton className="h-10 w-32 rounded-md" />
              ) : (
                <>
                  {hasPermission(PERMISSIONS.USER_EMAIL_UPDATE) ? (
                    <Button type="submit" variant="outline" className="cursor-pointer" disabled={true}>
                      Change email
                    </Button>
                  ) : (
                    <Tooltip delayDuration={400}>
                      <TooltipTrigger asChild>
                        <span className="inline-block">
                          <Button className="cursor-not-allowed" variant="outline" disabled>
                            Change email
                          </Button>
                        </span>
                      </TooltipTrigger>
                      <TooltipContent>
                        You don&apos;t have permission to change your email.
                      </TooltipContent>
                    </Tooltip>
                  )}
                </>
              )}
            </div>
        </div>
      </div>
      <p className="text-muted-foreground text-sm">We&apos;ll use this to contact you. Only members in the projects you are in will be able to see this email.</p>
    </div>
  );
}