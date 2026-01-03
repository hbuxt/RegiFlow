import { useMyDetails } from "@/hooks/useUser";
import { updateMyDetailsSchema, UpdateMyDetailsSchema } from "@/lib/schemas/user";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormProvider, useForm } from "react-hook-form";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { Skeleton } from "./ui/skeleton";
import { useEffect, useState } from "react";
import { ApiError } from "@/lib/utils/result";
import { updateMyDetails } from "@/lib/services/user";
import { useQueryClient } from "@tanstack/react-query";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { AlertCircleIcon, CheckCircle2Icon, Loader } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { useAuthorization } from "@/hooks/useAuthorization";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { toast } from "sonner";

export default function UpdateMyDetailsForm() {
  const queryClient = useQueryClient();
  const { data, isLoading: isMyDetailsPending, error: myDetailsError } = useMyDetails();
  const { hasPermission, isPending: isPermissionPending, error: permissionError } = useAuthorization();
  const [processing, setProcessing] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState<ApiError | null>(null);

  const form = useForm<UpdateMyDetailsSchema>({
    resolver: zodResolver(updateMyDetailsSchema),
    defaultValues: {
      firstName: "",
      lastName: ""
    }
  });

  useEffect(() => {
    if (myDetailsError) {
      toast.error("Failed to fetch your details", {
        description: myDetailsError.errors?.map(e => e.message).join(", ") ?? "",
        duration: Infinity
      });
    }

    if (permissionError) {
      toast.error("Failed to fetch your permissions", {
        description: permissionError.errors?.map(e => e.message).join(", ") ?? "",
        duration: Infinity
      });
    }
  }, [myDetailsError, permissionError]);

  useEffect(() => {
    if (data) {
      form.reset({
        firstName: data.firstName ?? "",
        lastName: data.lastName ?? ""
      });
    }
  }, [data, form]);

  async function onSubmit(values: UpdateMyDetailsSchema) {
    setProcessing(true);
    setSuccess(false);
    const response = await updateMyDetails(values);

    if (!response.success) {
      setProcessing(false);
      setSuccess(false);
      setError(response.error ?? null);
      return;
    }

    setProcessing(false);
    setSuccess(true);
    setError(null);
    queryClient.invalidateQueries({ queryKey: [QUERY_KEYS.GET_MY_DETAILS], exact: true });
  }

  return (
    <FormProvider {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        {success ? (
          <Alert className="border-emerald-600/50 text-emerald-600">
            <CheckCircle2Icon />
            <AlertTitle>Success! Your details have been saved</AlertTitle>
            <AlertDescription className="text-emerald-600">
              Changes should take effect immediately.
            </AlertDescription>
          </Alert>
        ) : (
          <>
            {error ? (
              <Alert variant="destructive">
                <AlertCircleIcon />
                <AlertTitle>Unable to update your details</AlertTitle>
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
          </>
        )}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormField control={form.control} name="firstName" render={({ field }: {field: any }) => (
            <FormItem className="gap-0 pt-4">
              <FormLabel className="mb-2">First name</FormLabel>
              <FormControl className="mb-3">
                <Input type="text" {...field} disabled={processing || isMyDetailsPending || myDetailsError || isPermissionPending || permissionError || !hasPermission(PERMISSIONS.USER_PROFILE_UPDATE)} />
              </FormControl>
              <FormDescription />
              <FormMessage />
            </FormItem>
          )} />
          <FormField control={form.control} name="lastName" render={({ field }: {field: any }) => (
            <FormItem className="gap-0 pt-4">
              <FormLabel className="mb-2">Last name</FormLabel>
              <FormControl className="mb-3">
                <Input type="text" {...field} disabled={processing || isMyDetailsPending || myDetailsError || isPermissionPending || permissionError || !hasPermission(PERMISSIONS.USER_PROFILE_UPDATE)} />
              </FormControl>
              <FormDescription />
              <FormMessage />
            </FormItem>
          )} />
        </div>
        <div>
          <FormDescription>Your name may appear around RegiFlow where you contribute or are mentioned. You can remove it at any time.</FormDescription>
          <div className="flex items-center justify-end">
            {isMyDetailsPending || myDetailsError || isPermissionPending || permissionError ? (
              <Skeleton className="h-10 w-32 rounded-md" />
            ) : (
              <>
                {hasPermission(PERMISSIONS.USER_PROFILE_UPDATE) ? (
                  <Button type="submit" variant="outline" className="cursor-pointer" disabled={processing}>
                    {processing ? (
                    <><Loader className="animate-spin" /><span>Saving...</span></>
                    ) : (
                      <span>Save changes</span>
                    )}
                  </Button>
                ) : (
                  <Tooltip delayDuration={400}>
                    <TooltipTrigger asChild>
                      <span className="inline-block">
                        <Button className="cursor-not-allowed" variant="outline" disabled>
                          Save changes
                        </Button>
                      </span>
                    </TooltipTrigger>
                    <TooltipContent>
                      You don&apos;t have permission to update your details.
                    </TooltipContent>
                  </Tooltip>
                )}
              </>
            )}
          </div>
        </div>
      </form>
    </FormProvider>
  );
}