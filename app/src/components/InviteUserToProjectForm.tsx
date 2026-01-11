import { PERMISSIONS } from "@/lib/constants/permissions";
import { Project } from "@/lib/types/project";
import { Button } from "./ui/button";
import { AlertCircleIcon, CheckCircle2Icon, Loader, UserPlus } from "lucide-react";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { AlertDialog, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "./ui/alert-dialog";
import { FormProvider, useForm } from "react-hook-form";
import { inviteUserToProjectSchema, InviteUserToProjectSchema } from "@/lib/schemas/project";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQuery } from "@tanstack/react-query";
import { AppError } from "@/lib/utils/errors";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { Role } from "@/lib/types/role";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { getRolesByScope } from "@/lib/services/role";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "./ui/select";
import { inviteUserToProject } from "@/lib/services/project";

interface InviteUserToProjectFormProps {
  project: Project;
  permissions: string[];
}

export function InviteUserToProjectForm(props: InviteUserToProjectFormProps) {
  const { data, isPending, isError } = useQuery<Role[], AppError>({
    queryKey: [QUERY_KEYS.GET_ROLES_BY_SCOPE, "project"],
    queryFn: () => getRolesByScope("project"),
    staleTime: 1000 * 60 * 3,
    refetchOnWindowFocus: false,
    retry: false
  });

  const form = useForm<InviteUserToProjectSchema>({
    resolver: zodResolver(inviteUserToProjectSchema),
    defaultValues: {
      id: props.project.id,
      email: "",
      role: ""
    }
  });

  const mutation = useMutation<undefined, AppError | Error, InviteUserToProjectSchema>({
    mutationFn: inviteUserToProject
  });

  function onSubmit(values: InviteUserToProjectSchema) {
    mutation.mutate(values);
  }

  return (
    <AlertDialog>
      {props.permissions.includes(PERMISSIONS.PROJECT_USERS_INVITE) ? (
        <AlertDialogTrigger asChild>
          <Button className="cursor-pointer">
            <UserPlus /> Invite
          </Button>
        </AlertDialogTrigger>
      ) : (
        <Tooltip delayDuration={400}>
          <TooltipTrigger asChild>
            <span className="inline-block">
              <Button className="cursor-pointer" disabled>
                <UserPlus /> Invite
              </Button>
            </span>
          </TooltipTrigger>
          <TooltipContent>
            You don&apos;t have permission to invite users.
          </TooltipContent>
        </Tooltip>
      )}
      <AlertDialogContent>
        <FormProvider {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)}>
            <AlertDialogHeader>
              <AlertDialogTitle>Invite a user</AlertDialogTitle>
              <AlertDialogDescription>
                If the email matches an account, the user will receive this invitation where they can either accept, or decline.
              </AlertDialogDescription>
              {mutation.isSuccess ? (
                <Alert className="border-emerald-600/50 text-emerald-600 my-2">
                  <CheckCircle2Icon />
                  <AlertTitle>Success! Invitation sent</AlertTitle>
                  <AlertDescription className="text-emerald-600">
                    If the email matches an existing account, the user will recieve an invitation.
                  </AlertDescription>
                </Alert>
              ) : null}
              {mutation.isError ? (
                <Alert variant="destructive">
                  <AlertCircleIcon />
                  <AlertTitle>Unable to send an invitation</AlertTitle>
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
              <FormField control={form.control} name="id" render={({ field }: {field: any }) => (
                <FormItem>
                  <FormControl>
                    <Input type="hidden" {...field} />
                  </FormControl>
                  <FormDescription />
                  <FormMessage />
                </FormItem>
              )} />
              <FormField control={form.control} name="email" render={({ field }: {field: any }) => (
                <FormItem className="gap-0 py-2">
                  <FormLabel className="mb-2">Email</FormLabel>
                  <FormControl className="mb-3">
                    <Input type="text" {...field} disabled={mutation.isPending} />
                  </FormControl>
                  <FormDescription />
                  <FormMessage />
                </FormItem>
              )} />
              <FormField control={form.control} name="role" render={({ field }: {field: any }) => (
                <FormItem className="gap-0 pt-2 pb-4">
                  <FormLabel className="mb-2">Role</FormLabel>
                  {isPending || isError ? (
                    <>Oops</>
                  ) : (
                    <Select {...field} onValueChange={field.onChange}>
                      <FormControl>
                        <SelectTrigger id="role" className="w-full mb-3">
                          <SelectValue placeholder="Select" />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {data.map((role) => (
                          <SelectItem key={role.id} value={role.id}>{role.name}</SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  )}
                  <FormDescription />
                  <FormMessage />
                </FormItem>
              )} />
            </AlertDialogHeader>
            <AlertDialogFooter>
              <AlertDialogCancel className="cursor-pointer" disabled={mutation.isPending}>
                Cancel
              </AlertDialogCancel>
              <Button type="submit" className="cursor-pointer" disabled={mutation.isPending}>
                {mutation.isPending ? (
                  <><Loader className="animate-spin" /><span>Sending invitation...</span></>
                ) : (
                  <span>Send</span>
                )}
              </Button>
            </AlertDialogFooter>
          </form>
        </FormProvider>
      </AlertDialogContent>
    </AlertDialog>
  );
}