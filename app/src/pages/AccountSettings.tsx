import ChangeMyEmailForm from "@/components/ChangeMyEmailForm";
import ChangeMyPasswordForm from "@/components/ChangeMyPasswordForm";
import DeleteMyAccountForm from "@/components/DeleteMyAccountForm";
import UpdateMyDetailsForm from "@/components/UpdateMyDetailsForm";

export default function AccountSettings() {
  return (
    <div className="flex flex-col gap-4">
      <div>
        <div className="w-full border-b pb-4">
          <h2 className="text-lg font-normal">Account</h2>
        </div>
      </div>
      <UpdateMyDetailsForm />
      <ChangeMyEmailForm />
      <ChangeMyPasswordForm />
      <DeleteMyAccountForm />
    </div>
  )
}