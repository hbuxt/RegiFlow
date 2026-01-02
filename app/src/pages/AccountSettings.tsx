import DeleteMyAccountForm from "@/components/DeleteMyAccountForm";

export default function AccountSettings() {
  return (
    <div className="flex flex-col gap-4">
      <div>
        <div className="w-full border-b pb-4">
          <h2 className="text-lg font-normal">Account</h2>
        </div>
      </div>
      <DeleteMyAccountForm />
    </div>
  )
}