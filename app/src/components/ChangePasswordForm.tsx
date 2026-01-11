import { Label } from "./ui/label";
import { Input } from "./ui/input";
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
        <Input id="password" type="password" defaultValue="0000-0000-0000-0000-0000" disabled={true} />
      </div>
    </div>
  );
}