import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { FormProvider, useForm } from "react-hook-form";
import { SignupSchema, signupSchema } from "@/lib/schemas/auth";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { AlertCircleIcon, Loader } from "lucide-react";
import { signup } from "@/lib/services/auth";
import { useAuthentication } from "@/hooks/useAuthentication";
import { useMutation } from "@tanstack/react-query";
import { AppError } from "@/lib/utils/errors";

export default function SignUpForm() {
  const { authenticate } = useAuthentication();

  const form = useForm<SignupSchema>({
    resolver: zodResolver(signupSchema),
    defaultValues: {
      email: "",
      password: "",
      confirmPassword: ""
    }
  });

  const mutation = useMutation<string | null, AppError | Error, SignupSchema>({
    mutationFn: signup,
    onSuccess: (session) => {
      if (!session) {
        window.location.href = "/account/login";
        return;
      }

      authenticate(session);
      window.location.href = "/";
    }
  });

  async function onSubmit(values: SignupSchema) {
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
      <Card>
        <CardHeader>
          <CardTitle>Create an account</CardTitle>
          <CardDescription>Enter your information below to create your account.</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={form.handleSubmit(onSubmit)}>
            {mutation.isError ? (
              <Alert variant="destructive">
                <AlertCircleIcon />
                <AlertTitle>Unable to create your account</AlertTitle>
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
                <FormDescription className="mb-2">We&apos;ll use this to contact you. Only members in the projects you are in will be able to see this email.</FormDescription>
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
            <FormField control={form.control} name="confirmPassword" render={({ field }: {field: any }) => (
              <FormItem className="gap-0 py-4">
                <FormLabel className="mb-2">Confirm password</FormLabel>
                <FormControl className="mb-2">
                  <Input type="password" disabled={mutation.isPending} {...field} />
                </FormControl>
                <FormDescription />
                <FormMessage className="mb-2" />
              </FormItem>
            )} />
            <Button type="submit" disabled={mutation.isPending} className="w-full cursor-pointer">
              {mutation.isPending ? (
                <><Loader className="animate-spin" /><span>Creating account...</span></>
              ) : "Create account"}
            </Button>
            <FormDescription className="mt-4 text-center">
              Already have an account? <a href="/account/login" className="text-primary hover:underline">Log in</a>
            </FormDescription>
          </form>
        </CardContent>
      </Card>
      <FormDescription className="py-4 px-6 text-center">
        By clicking continue, you agree to our Terms of Service and Privacy Policy
      </FormDescription>
    </FormProvider>
  );
}