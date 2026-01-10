import { updateProjectDescriptionSchema, UpdateProjectDescriptionSchema } from "@/lib/schemas/project";
import { Project } from "@/lib/types/project";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormProvider, useForm } from "react-hook-form";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { Textarea } from "./ui/textarea";
import { Button } from "./ui/button";
import { useState } from "react";

interface UpdateProjectDescriptionFormProps {
  project: Project,
  permissions: string[]
}

export default function UpdateProjectDescriptionForm(props: UpdateProjectDescriptionFormProps) {
  const characterMaxLength = 256;
  const [characterCounter, setCharacterCounter] = useState(props.project.description?.length ?? 0);

  const form = useForm<UpdateProjectDescriptionSchema>({
    resolver: zodResolver(updateProjectDescriptionSchema),
    defaultValues: {
      id: props.project.id,
      description: props.project.description ?? ""
    }
  });
  
  function onSubmit(values: UpdateProjectDescriptionSchema) {
    console.log(values);
  }
    
  return (
    <FormProvider {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="flex flex-col md:flex-row gap-4 justify-between py-3">
        <div className="flex flex-col flex-1 gap-2">
          <FormField control={form.control} name="id" render={({ field }: {field: any }) => (
            <FormItem>
              <FormControl>
                <Input type="hidden" {...field} />
              </FormControl>
              <FormDescription />
              <FormMessage />
            </FormItem>
          )} />
          <FormField control={form.control} name="description" render={({ field }: {field: any }) => (
            <FormItem className="gap-0 py-4">
              <FormLabel className="mb-2">Description</FormLabel>
              <div className="flex gap-4 items-end mb-1">
                <FormControl className="flex-1">
                  <Textarea rows={8} {...field} maxLength={characterMaxLength} onInput={(e: React.FormEvent<HTMLTextAreaElement>) => setCharacterCounter(e.currentTarget.value.length)} />
                </FormControl>
                <Button type="submit" variant="outline" className="h-fit cursor-pointer">
                  Update
                </Button>
              </div>
              <FormDescription>{characterCounter}/{characterMaxLength}</FormDescription>
              <FormMessage />
            </FormItem>
          )} />
        </div>
      </form>
    </FormProvider>
  );
}