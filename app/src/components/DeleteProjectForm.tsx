import { deleteProjectSchema, DeleteProjectSchema } from "@/lib/schemas/project";
import { Project } from "@/lib/types/project";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormProvider, useForm } from "react-hook-form";
import { AlertDialog, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "./ui/alert-dialog";
import { Button } from "./ui/button";
import { FormControl, FormDescription, FormField, FormItem, FormMessage } from "./ui/form";
import { Input } from "./ui/input";

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

  function onSubmit(values: DeleteProjectSchema) {
    console.log(values);
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
          <AlertDialogTrigger asChild>
            <Button className="cursor-pointer" variant="destructive">
              Delete Project
            </Button>
          </AlertDialogTrigger>
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
              <AlertDialogCancel className="cursor-pointer">
                Cancel
              </AlertDialogCancel>
              <Button type="submit" variant="destructive" className="cursor-pointer">
                Delete
              </Button>
            </AlertDialogFooter>
          </form>
        </FormProvider>
      </AlertDialogContent>
    </AlertDialog>
  );
}