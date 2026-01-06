import { Label } from "./ui/label";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { User } from "@/lib/types/user";

interface ChangeEmailFormProps {
  user: User;
  permissions: string[]
}

export default function ChangeEmailForm(props: ChangeEmailFormProps) {
  return (
    <div className="pb-3">
      <div className="flex flex-row gap-4 justify-between py-3">
        <div className="flex flex-col flex-1 gap-2">
          <Label htmlFor="email">Email</Label>
          <Input id="email" type="email" defaultValue={props.user.email} disabled={!props.permissions.includes(PERMISSIONS.USER_EMAIL_UPDATE)} />
        </div>
        <div className="flex items-end md:justify-end">
          <div className="flex items-center justify-end">
            {props.permissions.includes(PERMISSIONS.USER_EMAIL_UPDATE) ? (
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
          </div>
        </div>
      </div>
      <p className="text-muted-foreground text-sm">We&apos;ll use this to contact you. Only members in the projects you are in will be able to see this email.</p>
    </div>
  );
}