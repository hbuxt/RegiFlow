import { renameProjectSchema, RenameProjectSchema } from "@/lib/schemas/project";
import { Project } from "@/lib/types/project";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormProvider, useForm } from "react-hook-form";
import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from "./ui/alert-dialog";
import { Label } from "./ui/label";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";

interface RenameProjectFormProps {
  project: Project,
  permissions: string[]
};

export default function RenameProjectForm(props: RenameProjectFormProps) {
  const form = useForm<RenameProjectSchema>({
    resolver: zodResolver(renameProjectSchema),
    defaultValues: {
      id: props.project.id,
      name: ""
    }
  });

  function onSubmit(values: RenameProjectSchema) {
    console.log(values);
  }
  
  return (
    <AlertDialog>
      <div className="flex flex-row gap-4 justify-between pt-6">
        <div className="flex flex-col flex-1 gap-2">
          <Label htmlFor="name">Name</Label>
          <Input id="name" type="text" defaultValue={props.project.name ?? ""} disabled={true} />
        </div>
        <div className="flex items-end md:justify-end">
          <Button asChild className="cursor-pointer" variant="outline">
            <AlertDialogTrigger>Rename</AlertDialogTrigger>
          </Button>
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
                <FormField control={form.control} name="id" render={({ field }: {field: any }) => (
                  <FormItem>
                    <FormControl>
                      <Input type="hidden" {...field} />
                    </FormControl>
                    <FormDescription />
                    <FormMessage />
                  </FormItem>
                )} />
                <FormField control={form.control} name="name" render={({ field }: {field: any }) => (
                  <FormItem className="gap-0 py-4">
                    <FormLabel className="mb-2">Name</FormLabel>
                    <FormControl className="mb-3">
                      <Input type="text" {...field} placeholder={props.project.name} />
                    </FormControl>
                    <FormDescription />
                    <FormMessage />
                  </FormItem>
                )} />
            </AlertDialogHeader>
            <AlertDialogFooter>
              <AlertDialogCancel className="cursor-pointer">Cancel</AlertDialogCancel>
              <AlertDialogAction className="cursor-pointer" type="submit">Save Changes</AlertDialogAction>
            </AlertDialogFooter>
          </form>
        </FormProvider>
      </AlertDialogContent>
    </AlertDialog>
  );
}