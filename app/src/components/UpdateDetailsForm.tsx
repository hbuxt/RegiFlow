import { updateMyDetailsSchema, UpdateMyDetailsSchema } from "@/lib/schemas/user";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormProvider, useForm } from "react-hook-form";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { updateMyDetails } from "@/lib/services/user";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { AlertCircleIcon, CheckCircle2Icon, Loader } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { User } from "@/lib/types/user";
import { AppError } from "@/lib/utils/errors";

interface UpdateDetailsFormProps {
  user: User;
  permissions: string[]
}

export default function UpdateDetailsForm(props: UpdateDetailsFormProps) {
  const queryClient = useQueryClient();

  const form = useForm<UpdateMyDetailsSchema>({
    resolver: zodResolver(updateMyDetailsSchema),
    defaultValues: {
      firstName: props.user.firstName ?? "",
      lastName: props.user.lastName ?? ""
    }
  });

  const mutation = useMutation<undefined, AppError | Error, UpdateMyDetailsSchema>({
    mutationFn: updateMyDetails,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [QUERY_KEYS.GET_MY_DETAILS], exact: true });
    }
  })

  async function onSubmit(values: UpdateMyDetailsSchema) {
    mutation.mutate(values);
  }

  return (
    <FormProvider {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        {mutation.isSuccess ? (
          <Alert className="border-emerald-600/50 text-emerald-600">
            <CheckCircle2Icon />
            <AlertTitle>Success! Your details have been saved</AlertTitle>
            <AlertDescription className="text-emerald-600">
              Changes should take effect immediately.
            </AlertDescription>
          </Alert>
        ) : (
          <>
            {mutation.isError ? (
              <Alert variant="destructive">
                <AlertCircleIcon />
                <AlertTitle>Unable to update your details</AlertTitle>
                <AlertDescription>
                  {mutation.error instanceof AppError ? (
                    mutation.error.details.length === 1 ? (
                      <p>{mutation.error.details[0].message}</p>
                    ) : (
                      <ul className="list-inside list-disc text-sm">
                        {mutation.error.details.map((error, index) => (
                          <li key={index}>{error.message}</li>
                        ))}
                      </ul>
                    )
                  ) : (
                    <p>Something went wrong. Please try again later.</p>
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
                <Input type="text" {...field} disabled={mutation.isPending || !props.permissions.includes(PERMISSIONS.USER_PROFILE_UPDATE)} />
              </FormControl>
              <FormDescription />
              <FormMessage />
            </FormItem>
          )} />
          <FormField control={form.control} name="lastName" render={({ field }: {field: any }) => (
            <FormItem className="gap-0 pt-4">
              <FormLabel className="mb-2">Last name</FormLabel>
              <FormControl className="mb-3">
                <Input type="text" {...field} disabled={mutation.isPending || !props.permissions.includes(PERMISSIONS.USER_PROFILE_UPDATE)} />
              </FormControl>
              <FormDescription />
              <FormMessage />
            </FormItem>
          )} />
        </div>
        <div>
          <FormDescription>Your name may appear around RegiFlow where you contribute or are mentioned. You can remove it at any time.</FormDescription>
          <div className="flex items-center justify-end pt-3">
            {props.permissions.includes(PERMISSIONS.USER_PROFILE_UPDATE) ? (
              <Button type="submit" variant="outline" className="cursor-pointer" disabled={mutation.isPending}>
                {mutation.isPending ? (
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
          </div>
        </div>
      </form>
    </FormProvider>
  );
}