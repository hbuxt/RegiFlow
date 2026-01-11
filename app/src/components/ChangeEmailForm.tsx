import { Label } from "./ui/label";
import { Input } from "./ui/input";
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
          <Input id="email" type="email" defaultValue={props.user.email} disabled={true} />
        </div>
      </div>
      <p className="text-muted-foreground text-sm">We&apos;ll use this to contact you. Only members in the projects you are in will be able to see this email.</p>
    </div>
  );
}