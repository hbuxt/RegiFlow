import { renameProjectSchema, RenameProjectSchema } from "@/lib/schemas/project";
import { Project } from "@/lib/types/project";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormProvider, useForm } from "react-hook-form";
import { AlertDialog, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "./ui/alert-dialog";
import { Label } from "./ui/label";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { useMutation } from "@tanstack/react-query";
import { AppError } from "@/lib/utils/errors";
import { renameProject } from "@/lib/services/project";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { AlertCircleIcon, CheckCircle2Icon, Loader } from "lucide-react";
import { useLocation } from "react-router";

interface RenameProjectFormProps {
  project: Project,
  permissions: string[]
};

export default function RenameProjectForm(props: RenameProjectFormProps) {
  const location = useLocation();
  const form = useForm<RenameProjectSchema>({
    resolver: zodResolver(renameProjectSchema),
    defaultValues: {
      id: props.project.id,
      name: ""
    }
  });

  const mutation = useMutation<undefined, AppError | Error, RenameProjectSchema>({
    mutationFn: renameProject,
    onSuccess: () => {
      window.location.href = location.pathname;
    }
  });

  function onSubmit(values: RenameProjectSchema) {
    mutation.mutate(values);
  }
  
  return (
    <AlertDialog>
      <div className="flex flex-row gap-4 justify-between pt-6">
        <div className="flex flex-col flex-1 gap-2">
          <Label htmlFor="name">Name</Label>
          <Input id="name" type="text" defaultValue={props.project.name ?? ""} disabled={true} />
        </div>
        <div className="flex items-end md:justify-end">
          {props.permissions.includes(PERMISSIONS.PROJECT_NAME_UPDATE) ? (
            <Button asChild className="cursor-pointer" variant="outline">
              <AlertDialogTrigger>Rename</AlertDialogTrigger>
            </Button>
          ) : (
            <Tooltip delayDuration={400}>
              <TooltipTrigger asChild>
                <span className="inline-block">
                  <Button className="cursor-not-allowed" variant="outline" disabled>
                    Rename
                  </Button>
                </span>
              </TooltipTrigger>
              <TooltipContent>
                You don&apos;t have permission to rename this project.
              </TooltipContent>
            </Tooltip>
          )}
        </div>
      </div>
      <AlertDialogContent>
        <FormProvider {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)}>
            <AlertDialogHeader>
              <AlertDialogTitle>Rename project</AlertDialogTitle>
                <AlertDialogDescription>
                  Provide a new name for your project.
                </AlertDialogDescription>
                  {mutation.isSuccess ? (
                    <Alert className="border-emerald-600/50 text-emerald-600">
                      <CheckCircle2Icon />
                      <AlertTitle>Success! Project has been renamed</AlertTitle>
                      <AlertDescription className="text-emerald-600">
                        Changes should take effect immediately.
                      </AlertDescription>
                    </Alert>
                  ) : (
                    <>
                      {mutation.isError ? (
                        <Alert variant="destructive">
                          <AlertCircleIcon />
                          <AlertTitle>Unable to rename project</AlertTitle>
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
                <FormField control={form.control} name="id" render={({ field }: {field: any }) => (
                  <FormItem>
                    <FormControl>
                      <Input type="hidden" {...field} disabled={mutation.isPending} />
                    </FormControl>
                    <FormDescription />
                    <FormMessage />
                  </FormItem>
                )} />
                <FormField control={form.control} name="name" render={({ field }: {field: any }) => (
                  <FormItem className="gap-0 py-4">
                    <FormLabel className="mb-2">Name</FormLabel>
                    <FormControl className="mb-3">
                      <Input type="text" {...field} disabled={mutation.isPending} placeholder={props.project.name} />
                    </FormControl>
                    <FormDescription />
                    <FormMessage />
                  </FormItem>
                )} />
            </AlertDialogHeader>
            <AlertDialogFooter>
              <AlertDialogCancel className="cursor-pointer" disabled={mutation.isPending}>Cancel</AlertDialogCancel>
              <Button type="submit" className="cursor-pointer" disabled={mutation.isPending}>
                {mutation.isPending ? (
                  <><Loader className="animate-spin" /><span>Saving changes...</span></>
                ) : (
                  <span>Save changes</span>
                )}
              </Button>
            </AlertDialogFooter>
          </form>
        </FormProvider>
      </AlertDialogContent>
    </AlertDialog>
  );
}