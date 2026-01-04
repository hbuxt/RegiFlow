import { deleteMyAccountSchema, DeleteMyAccountSchema } from "@/lib/schemas/user";
import { deleteMyAccount } from "@/lib/services/user";
import { ApiError } from "@/lib/utils/result";
import { zodResolver } from "@hookform/resolvers/zod";
import { useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { FormProvider, useForm } from "react-hook-form";
import { AlertDialog, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "./ui/alert-dialog";
import { Button } from "./ui/button";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { AlertCircleIcon, Loader } from "lucide-react";
import { FormControl, FormDescription, FormField, FormItem, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { useAuthentication } from "@/hooks/useAuthentication";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { User } from "@/lib/types/user";

interface DeleteUserAccountFormProps {
  user: User;
  permissions: string[];
}

export default function DeleteMyAccountForm(props: DeleteUserAccountFormProps) {
  const queryClient = useQueryClient();
  const { deauthenticate } = useAuthentication();
  const [processing, setProcessing] = useState(false);
  const [error, setError] = useState<ApiError | null>(null);
  
  const form = useForm<DeleteMyAccountSchema>({
    resolver: zodResolver(deleteMyAccountSchema),
    defaultValues: {
      id: props.user.id
    }
  });

  async function onSubmit(values: DeleteMyAccountSchema) {
    setProcessing(true);
    const response = await deleteMyAccount(values);

    if (!response.success) {
      setProcessing(false);
      setError(response.error ?? null);
      return;
    }

    deauthenticate();
    queryClient.clear();
    window.location.href = "/";
  }
  
  return (
    <AlertDialog>
      <div className="flex flex-col md:flex-row gap-4 justify-between pt-6">
        <div className="flex flex-col flex-1 gap-1">
          <h4 className="text-md font-medium text-destructive">Delete account</h4>
          <p className="text-muted-foreground text-sm">
            Permanently delete your account. This action cannot be undone.
          </p>
        </div>
        <div className="flex md:justify-end md:items-center">
          {props.permissions.includes(PERMISSIONS.USER_DELETE) ? (
            <AlertDialogTrigger asChild>
              <Button className="cursor-pointer" variant="destructive">
                Delete Account
              </Button>
            </AlertDialogTrigger>
          ) : (
            <Tooltip delayDuration={400}>
              <TooltipTrigger asChild>
                <span className="inline-block">
                  <AlertDialogTrigger asChild>
                    <Button className="cursor-not-allowed" variant="destructive" disabled>
                      Delete Account
                    </Button>
                  </AlertDialogTrigger>
                </span>
              </TooltipTrigger>
              <TooltipContent>
                You don&apos;t have permission to delete your account.
              </TooltipContent>
            </Tooltip>
          )}
        </div>
      </div>
      <AlertDialogContent>
        <FormProvider {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)}>
            <AlertDialogHeader>
              <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
              <AlertDialogDescription>
                This action cannot be undone. This will permanently delete your account
                and any related information.
              </AlertDialogDescription>
              {error ? (
                <Alert variant="destructive">
                  <AlertCircleIcon />
                  <AlertTitle>Unable to delete your account</AlertTitle>
                  <AlertDescription>
                    {error.errors.length == 1 ? (
                      <p>{error.errors[0].message}</p>
                    ) : (
                      <ul className="list-inside list-disc text-sm">
                        {error.errors.map((error, index) => (
                          <li key={index}>{error.message}</li>
                        ))}
                      </ul>
                    )}
                  </AlertDescription>
                </Alert>
              ) : null}
              <FormField control={form.control} name="id" render={({ field }: {field: any }) => (
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
              <AlertDialogCancel className="cursor-pointer" disabled={processing}>
                Cancel
              </AlertDialogCancel>
              <Button type="submit" variant="destructive" className="cursor-pointer" disabled={processing}>
                {processing ? (
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
  );
}