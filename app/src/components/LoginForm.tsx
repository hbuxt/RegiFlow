import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "./ui/card";
import { FormProvider, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AlertCircleIcon, Loader } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Button } from "./ui/button";
import { Input } from "./ui/input";
import { loginSchema, LoginSchema } from "@/lib/schemas/auth";
import { login } from "@/lib/services/auth";
import { useAuthentication } from "@/hooks/useAuthentication";
import { useMutation } from "@tanstack/react-query";
import { AppError } from "@/lib/utils/errors";

export default function LoginForm() {
  const { authenticate } = useAuthentication();

  const form = useForm<LoginSchema>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: "",
      password: ""
    }
  });

  const mutation = useMutation<string, AppError | Error, LoginSchema>({
    mutationFn: login,
    onSuccess: (session) => {
      authenticate(session);
      window.location.href = "/";
    }
  });

  async function onSubmit(values: LoginSchema) {
    mutation.mutate(values);
  }

  if (mutation.isSuccess) {
    return (
      <Card>
        <CardHeader className="flex flex-col-reverse justify-center items-center">
          <CardTitle className="font-normal text-muted-foreground">Signing you in...</CardTitle>
          <CardDescription><Loader className="animate-spin" /></CardDescription>
        </CardHeader>
      </Card>
    );
  }

  return (
    <FormProvider {...form}>
      <div className="flex flex-col gap-6">
        <Card>
          <CardHeader className="text-center">
            <CardTitle className="text-xl">Welcome back</CardTitle>
            <CardDescription>
              Login with your account
            </CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={form.handleSubmit(onSubmit)}>
              {mutation.isError ? (
                <Alert variant="destructive">
                  <AlertCircleIcon />
                  <AlertTitle>Unable to sign you in</AlertTitle>
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
              <FormField control={form.control} name="email" render={({ field }: {field: any }) => (
                <FormItem className="gap-0 py-4">
                  <FormLabel className="mb-2">Email</FormLabel>
                  <FormControl className="mb-2">
                    <Input type="text" disabled={mutation.isPending} {...field} />
                  </FormControl>
                  <FormDescription />
                  <FormMessage />
                </FormItem>
              )} />
              <FormField control={form.control} name="password" render={({ field }: {field: any }) => (
                <FormItem className="gap-0 py-4">
                  <FormLabel className="mb-2">Password</FormLabel>
                  <FormControl className="mb-2">
                    <Input type="password" disabled={mutation.isPending} {...field} />
                  </FormControl>
                  <FormDescription />
                  <FormMessage className="mb-2" />
                </FormItem>
              )} />
              <Button type="submit" disabled={mutation.isPending} className="w-full cursor-pointer">
                {mutation.isPending ? (
                  <><Loader className="animate-spin" /><span>Signing you in...</span></>
                ) : "Login"}
              </Button>
              <FormDescription className="mt-4 text-center">
                Don&apos;t have an account? <a href="/account/sign-up" className="text-primary hover:underline">Sign up</a>
              </FormDescription>
            </form>
          </CardContent>
        </Card>
        <FormDescription className="px-6 text-center">
          By clicking continue, you agree to our Terms of Service and Privacy Policy
        </FormDescription>
      </div>
    </FormProvider>
  )
}