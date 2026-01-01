import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AlertDialog, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "@/components/ui/alert-dialog";
import { Button } from "@/components/ui/button";
import { FormControl, FormDescription, FormField, FormItem, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useAuth } from "@/contexts/AuthContext";
import { deleteAccountSchema, DeleteAccountSchema } from "@/lib/schemas/user";
import { deleteMyAccount } from "@/lib/services/user";
import { ApiError } from "@/lib/utils/result";
import { zodResolver } from "@hookform/resolvers/zod";
import { useQueryClient } from "@tanstack/react-query";
import { AlertCircleIcon, Loader } from "lucide-react";
import { useState } from "react";
import { FormProvider, useForm } from "react-hook-form";

export default function AccountSettings() {
  const { session, deauthenticate } = useAuth();
  const queryClient = useQueryClient();
  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState<ApiError | null>(null);
  
  const deleteAccountForm = useForm<DeleteAccountSchema>({
    resolver: zodResolver(deleteAccountSchema),
    defaultValues: {
      id: session?.id
    }
  });

  async function onDeleteAccount(values: DeleteAccountSchema) {
    setDeleting(true);
    const response = await deleteMyAccount(values);

    if (!response.success) {
      setDeleting(false);
      setDeleteError(response.error ?? null);
      return;
    }

    deauthenticate();
    queryClient.clear();
    window.location.href = "/";
  }

  return (
    <div className="flex flex-col gap-4">
      <div>
        <div className="w-full border-b pb-4">
          <h2 className="text-lg font-normal">Account</h2>
        </div>
      </div>
      <AlertDialog>
        <div className="flex flex-col md:flex-row gap-4 justify-between pt-6">
          <div className="flex flex-col flex-1 gap-1">
            <h4 className="text-md font-medium text-destructive">Delete account</h4>
            <p className="text-muted-foreground text-sm">
              Permanently delete your account. This action cannot be undone.
            </p>
          </div>
          <div className="flex md:justify-end md:items-center">
            <AlertDialogTrigger asChild>
              <Button className="cursor-pointer" variant="destructive">
                Delete Account
              </Button>
            </AlertDialogTrigger>
          </div>
        </div>
        <AlertDialogContent>
          <FormProvider {...deleteAccountForm}>
            <form onSubmit={deleteAccountForm.handleSubmit(onDeleteAccount)}>
              <AlertDialogHeader>
                <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
                <AlertDialogDescription>
                  This action cannot be undone. This will permanently delete your account
                  and any related information.
                </AlertDialogDescription>
                {deleteError ? (
                  <Alert variant="destructive">
                    <AlertCircleIcon />
                    <AlertTitle>Unable to delete your account</AlertTitle>
                    <AlertDescription>
                      {deleteError.errors.length == 1 ? (
                        <p>{deleteError.errors[0].message}</p>
                      ) : (
                        <ul className="list-inside list-disc text-sm">
                          {deleteError.errors.map((error, index) => (
                            <li key={index}>{error.message}</li>
                          ))}
                        </ul>
                      )}
                    </AlertDescription>
                  </Alert>
                ) : null}
                <FormField control={deleteAccountForm.control} name="id" render={({ field }: {field: any }) => (
                  <FormItem>
                    <FormControl>
                      <Input type="hidden" {...field} />
                    </FormControl>
                    <FormDescription />
                    <FormMessage />
                  </FormItem>
                )} />
              </AlertDialogHeader>
              <AlertDialogFooter>
                <AlertDialogCancel className="cursor-pointer" disabled={deleting}>
                  Cancel
                </AlertDialogCancel>
                <Button type="submit" variant="destructive" className="cursor-pointer" disabled={deleting}>
                  {deleting ? (
                    <><Loader className="animate-spin" /><span>Deleting your account...</span></>
                  ) : (
                    <span>Delete</span>
                  )}
                </Button>
              </AlertDialogFooter>
            </form>
          </FormProvider>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  )
}