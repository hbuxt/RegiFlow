import { deleteProjectSchema, DeleteProjectSchema } from "@/lib/schemas/project";
import { Project } from "@/lib/types/project";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormProvider, useForm } from "react-hook-form";
import { AlertDialog, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "./ui/alert-dialog";
import { Button } from "./ui/button";
import { FormControl, FormDescription, FormField, FormItem, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { useMutation } from "@tanstack/react-query";
import { AppError } from "@/lib/utils/errors";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { AlertCircleIcon, Loader } from "lucide-react";
import { deleteProjectById } from "@/lib/services/project";

interface DeleteProjectFormProps {
  project: Project,
  permissions: string[]
}

export default function DeleteProjectForm(props: DeleteProjectFormProps) {
  const form = useForm<DeleteProjectSchema>({
    resolver: zodResolver(deleteProjectSchema),
    defaultValues: {
      id: props.project.id
    }
  });

  const mutation = useMutation<undefined, AppError | Error, DeleteProjectSchema>({
    mutationFn: deleteProjectById,
    onSuccess: () => {
      window.location.href = "/"
    }
  });

  function onSubmit(values: DeleteProjectSchema) {
    mutation.mutate(values);
  }

  return (
    <AlertDialog>
      <div className="flex flex-col md:flex-row gap-4 justify-between py-3">
        <div className="flex flex-col flex-1 gap-1">
          <h4 className="text-md font-medium text-destructive">Delete project</h4>
          <p className="text-muted-foreground text-sm">
            Permanently delete this project and remove access for all users.
          </p>
        </div>
        <div className="flex md:justify-end md:items-center">
          {props.permissions.includes(PERMISSIONS.PROJECT_DELETE) ? (
            <AlertDialogTrigger asChild>
              <Button className="cursor-pointer" variant="destructive">
                Delete Project
              </Button>
            </AlertDialogTrigger>
          ) : (
            <Tooltip delayDuration={400}>
              <TooltipTrigger asChild>
                <span className="inline-block">
                  <AlertDialogTrigger asChild>
                    <Button className="cursor-not-allowed" variant="destructive" disabled>
                      Delete Project
                    </Button>
                  </AlertDialogTrigger>
                </span>
              </TooltipTrigger>
              <TooltipContent>
                You don&apos;t have permission to delete this project.
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
                This action cannot be undone. This will permanently delete project <span className="text-black font-bold mr-1">{props.project.name}</span>
                and delete any related information, including removing access for all users.
              </AlertDialogDescription>
              {mutation.isError ? (
                <Alert variant="destructive">
                  <AlertCircleIcon />
                  <AlertTitle>Unable to delete project</AlertTitle>
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
                    <Input type="hidden" {...field} disabled={mutation.isPending} />
                  </FormControl>
                  <FormDescription />
                  <FormMessage />
                </FormItem>
              )} />
            </AlertDialogHeader>
            <AlertDialogFooter>
              <AlertDialogCancel className="cursor-pointer" disabled={mutation.isPending}>
                Cancel
              </AlertDialogCancel>
              <Button type="submit" variant="destructive" className="cursor-pointer" disabled={mutation.isPending}>
                {mutation.isPending ? (
                  <><Loader className="animate-spin" /><span>Deleting project...</span></>
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