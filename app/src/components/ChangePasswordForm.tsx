import { Label } from "./ui/label";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { User } from "@/lib/types/user";

interface ChangePasswordFormProps {
  user: User;
  permissions: string[];
}

export default function ChangePasswordForm(props: ChangePasswordFormProps) {
  return (
    <div className="flex flex-row gap-4 justify-between pt-3">
      <div className="flex flex-col flex-1 gap-2">
        <Label htmlFor="email">Password</Label>
        <Input id="password" type="password" defaultValue="0000-0000-0000-0000-0000" disabled={!props.permissions.includes(PERMISSIONS.USER_PASSWORD_UPDATE)} />
      </div>
      <div className="flex items-end md:justify-end">
        <div className="flex items-center justify-end">
          {props.permissions.includes(PERMISSIONS.USER_PASSWORD_UPDATE) ? (
            <Button type="submit" variant="outline" className="cursor-pointer" disabled={true}>
              Change password
            </Button>
          ) : (
            <Tooltip delayDuration={400}>
              <TooltipTrigger asChild>
                <span className="inline-block">
                  <Button className="cursor-not-allowed" variant="outline" disabled>
                    Change password
                  </Button>
                </span>
              </TooltipTrigger>
              <TooltipContent>
                You don&apos;t have permission to change your password.
              </TooltipContent>
            </Tooltip>
          )}
        </div>
      </div>
    </div>
  );
}