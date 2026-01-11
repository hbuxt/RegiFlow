import { updateProjectDescriptionSchema, UpdateProjectDescriptionSchema } from "@/lib/schemas/project";
import { Project } from "@/lib/types/project";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormProvider, useForm } from "react-hook-form";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { Textarea } from "./ui/textarea";
import { Button } from "./ui/button";
import { useState } from "react";
import { useMutation } from "@tanstack/react-query";
import { AppError } from "@/lib/utils/errors";
import { updateProjectDescription } from "@/lib/services/project";
import queryClient from "@/lib/utils/tanstack";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { AlertCircleIcon, CheckCircle2Icon, Loader } from "lucide-react";
import { PERMISSIONS } from "@/lib/constants/permissions";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";

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

  const mutation = useMutation<undefined, AppError | Error, UpdateProjectDescriptionSchema>({
    mutationFn: updateProjectDescription,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [QUERY_KEYS.GET_PROJECT_BY_ID, props.project.id]
      });
    }
  });
  
  function onSubmit(values: UpdateProjectDescriptionSchema) {
    mutation.mutate(values);
  }
    
  return (
    <FormProvider {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="flex flex-col md:flex-row gap-4 justify-between py-3">
        <div className="flex flex-col flex-1 gap-2">
          {mutation.isSuccess ? (
            <Alert className="border-emerald-600/50 text-emerald-600">
              <CheckCircle2Icon />
              <AlertTitle>Success! Project has been updated</AlertTitle>
              <AlertDescription className="text-emerald-600">
                Changes should take effect immediately.
              </AlertDescription>
            </Alert>
          ) : (
            <>
              {mutation.isError ? (
                <Alert variant="destructive">
                  <AlertCircleIcon />
                  <AlertTitle>Unable to update project</AlertTitle>
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
                <Input type="hidden" {...field} disabled={mutation.isPending || !props.permissions.includes(PERMISSIONS.PROJECT_DESCRIPTION_UPDATE)} />
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
                  <Textarea rows={8} {...field} disabled={mutation.isPending || !props.permissions.includes(PERMISSIONS.PROJECT_DESCRIPTION_UPDATE)} maxLength={characterMaxLength} onInput={(e: React.FormEvent<HTMLTextAreaElement>) => setCharacterCounter(e.currentTarget.value.length)} />
                </FormControl>
                {props.permissions.includes(PERMISSIONS.PROJECT_DESCRIPTION_UPDATE) ? (
                  <Button type="submit" variant="outline" className="cursor-pointer" disabled={mutation.isPending}>
                    {mutation.isPending ? (
                      <><Loader className="animate-spin" /><span>Updating...</span></>
                    ) : (
                      <span>Update</span>
                    )}
                  </Button>
                ) : (
                  <Tooltip delayDuration={400}>
                    <TooltipTrigger asChild>
                      <span className="inline-block">
                        <Button className="cursor-not-allowed" variant="outline" disabled>
                          Update
                        </Button>
                      </span>
                    </TooltipTrigger>
                    <TooltipContent>
                      You don&apos;t have permission to update this project.
                    </TooltipContent>
                  </Tooltip>
                )}
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