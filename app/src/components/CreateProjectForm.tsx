import { NavLink, useNavigate } from "react-router";
import { Button } from "./ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "./ui/card";
import { useState } from "react";
import { ApiError } from "@/lib/utils/result";
import { FormProvider, useForm } from "react-hook-form";
import { createProjectSchema, CreateProjectSchema } from "@/lib/schemas/project";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { AlertCircleIcon, Loader } from "lucide-react";
import { createProject } from "@/lib/services/project";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { useQueryClient } from "@tanstack/react-query";
import { QUERY_KEYS } from "@/lib/constants/queryKeys";
import { Textarea } from "./ui/textarea";

export default function CreateProjectForm() {
  const characterMaxLength = 256;

  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [characterCounter, setCharacterCounter] = useState(0);
  const [processing, setProcessing] = useState(false);
  const [error, setError] = useState<ApiError | null>(null);

  const form = useForm<CreateProjectSchema>({
    resolver: zodResolver(createProjectSchema),
    defaultValues: {
      name: "",
      description: ""
    }
  });

  async function onSubmit(values: CreateProjectSchema) {
    setProcessing(true);
    const response = await createProject(values);

    if (!response.success) {
      setProcessing(false);
      setError(response.error ?? null);
      return;
    }

    queryClient.invalidateQueries({ queryKey: [QUERY_KEYS.GET_MY_PROJECTS], exact: true });
    navigate(`/project/${response.value?.id}`);
  }

  return (
    <FormProvider {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <Card>
          <CardHeader>
            <CardTitle>Create a project</CardTitle>
            <CardDescription>
              Create a project to structure you team's workflow.
            </CardDescription>
          </CardHeader>
          <CardContent>
            {error ? (
              <Alert variant="destructive">
                <AlertCircleIcon />
                <AlertTitle>Unable to create project</AlertTitle>
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
            <FormField control={form.control} name="name" render={({ field }: {field: any }) => (
              <FormItem className="gap-0 py-4">
                <FormLabel className="mb-2">Name</FormLabel>
                <FormControl className="mb-3">
                  <Input type="text" {...field} disabled={processing} />
                </FormControl>
                <FormDescription>A unique name for your project. Don&apos;t worry, you can change this later.</FormDescription>
                <FormMessage />
              </FormItem>
            )} />
            <FormField control={form.control} name="description" render={({ field }: {field: any }) => (
              <FormItem className="gap-0 pt-4">
                <FormLabel className="mb-2">Description</FormLabel>
                <FormControl className="mb-1">
                  <Textarea {...field} rows={8} maxLength={characterMaxLength} onInput={(e: React.FormEvent<HTMLTextAreaElement>) => setCharacterCounter(e.currentTarget.value.length)} disabled={processing} />
                </FormControl>
                <FormDescription>{characterCounter}/{characterMaxLength}</FormDescription>
                <FormMessage />
              </FormItem>
            )} />
          </CardContent>
          <CardFooter className="flex justify-between border-t">
            <Button type="submit" variant="outline" asChild>
              <NavLink to="/">
                Back
              </NavLink>
            </Button>
            <Button type="submit" variant="default" className="cursor-pointer" disabled={processing}>
              {processing ? (
                <><Loader className="animate-spin" /><span>Creating...</span></>
              ) : (
                <span>Create project</span>
              )}
            </Button>
          </CardFooter>
        </Card>
      </form>
    </FormProvider>
  );
}