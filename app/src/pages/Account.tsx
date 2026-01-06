import ChangeEmailForm from "@/components/ChangeEmailForm";
import ChangePasswordForm from "@/components/ChangePasswordForm";
import DeleteAccountForm from "@/components/DeleteAccountForm";
import UpdateDetailsForm from "@/components/UpdateDetailsForm";
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
      <UpdateDetailsForm user={user} permissions={permissions} />
      <ChangeEmailForm user={user} permissions={permissions} />
      <ChangePasswordForm user={user} permissions={permissions} />
      <DeleteAccountForm user={user} permissions={permissions} />
    </div>
  )
}