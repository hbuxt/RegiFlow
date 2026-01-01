import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { FormProvider, useForm } from "react-hook-form";
import { SignupSchema, signupSchema } from "@/lib/features/auth/signup/schema";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Input } from "./ui/input";
import { Button } from "./ui/button";
import { signup } from "@/lib/features/auth/signup/service";
import { useAuth } from "@/contexts/AuthContext";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { AlertCircleIcon, Loader } from "lucide-react";
import { ApiError } from "@/lib/shared/utils/result";

export default function SignUpForm() {
  const { login } = useAuth();
  const [processing, setProcessing] = useState(false);
  const [signingIn, setSigningIn] = useState(false);
  const [error, setError] = useState<ApiError | null>(null);

  const signupForm = useForm<SignupSchema>({
    resolver: zodResolver(signupSchema),
    defaultValues: {
      email: "",
      password: "",
      confirmPassword: ""
    }
  });

  async function onSignup(values: SignupSchema) {
    setProcessing(true);
    setSigningIn(false);

    const response = await signup(values);

    if (!response.success) {
      setProcessing(false);
      setSigningIn(false);
      setError(response.error ?? null);

      return;
    }

    if (response.value && response.value.accessToken) {
      setSigningIn(true);
      setTimeout(() => {
        login(response.value!.accessToken!);
        window.location.href = "/";
      }, 3000);
      return;
    }

    setTimeout(() => {
      window.location.href = "/account/login";
    }, 3000);
  }

  if (signingIn) {
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
    <FormProvider {...signupForm}>
      <Card>
        <CardHeader>
          <CardTitle>Create an account</CardTitle>
          <CardDescription>Enter your information below to create your account.</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={signupForm.handleSubmit(onSignup)}>
            {error ? (
              <Alert variant="destructive">
                <AlertCircleIcon />
                <AlertTitle>Unable to create your account</AlertTitle>
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
            <FormField control={signupForm.control} name="email" render={({ field }: {field: any }) => (
              <FormItem className="gap-0 py-4">
                <FormLabel className="mb-2">Email</FormLabel>
                <FormControl className="mb-2">
                  <Input type="text" disabled={processing} {...field} />
                </FormControl>
                <FormDescription className="mb-2">We&apos;ll use this to contact you. Only members in the projects you are in will be able to see this email.</FormDescription>
                <FormMessage />
              </FormItem>
            )} />
            <FormField control={signupForm.control} name="password" render={({ field }: {field: any }) => (
              <FormItem className="gap-0 py-4">
                <FormLabel className="mb-2">Password</FormLabel>
                <FormControl className="mb-2">
                  <Input type="password" disabled={processing} {...field} />
                </FormControl>
                <FormDescription />
                <FormMessage className="mb-2" />
              </FormItem>
            )} />
            <FormField control={signupForm.control} name="confirmPassword" render={({ field }: {field: any }) => (
              <FormItem className="gap-0 py-4">
                <FormLabel className="mb-2">Confirm password</FormLabel>
                <FormControl className="mb-2">
                  <Input type="password" disabled={processing} {...field} />
                </FormControl>
                <FormDescription />
                <FormMessage className="mb-2" />
              </FormItem>
            )} />
            <Button type="submit" disabled={processing} className="w-full cursor-pointer">
              {processing ? (
                <><Loader className="animate-spin" /><span>Creating account...</span></>
              ) : "Create account"}
            </Button>
            <FormDescription className="mt-4 text-center">
              Already have an account? <a href="#" className="text-primary hover:underline">Log in</a>
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