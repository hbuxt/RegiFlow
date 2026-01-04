import ChangeUserEmailForm from "@/components/ChangeUserEmailForm";
import ChangeUserPasswordForm from "@/components/ChangeUserPasswordForm";
import DeleteUserAccountForm from "@/components/DeleteUserAccountForm";
import UpdateUserDetailsForm from "@/components/UpdateUserDetailsForm";
import { User } from "@/lib/types/user";
import { useOutletContext } from "react-router";

export default function Account() {
  const { user, permissions } = useOutletContext() as { user: User, permissions: string[] };

  return (
    <div className="flex flex-col gap-4">
      <div>
        <div className="w-full border-b pb-4">
          <h2 className="text-lg font-normal">Account</h2>
        </div>
      </div>
      <UpdateUserDetailsForm user={user} permissions={permissions} />
      <ChangeUserEmailForm user={user} permissions={permissions} />
      <ChangeUserPasswordForm user={user} permissions={permissions} />
      <DeleteUserAccountForm user={user} permissions={permissions} />
    </div>
  )
}