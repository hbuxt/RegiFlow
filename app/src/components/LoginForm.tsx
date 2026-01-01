import { useAuth } from "@/contexts/AuthContext";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "./ui/card";
import { useState } from "react";
import { ApiError } from "@/lib/utils/result";
import { FormProvider, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AlertCircleIcon, Loader } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "./ui/alert";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "./ui/form";
import { Button } from "./ui/button";
import { Input } from "./ui/input";
import { loginSchema, LoginSchema } from "@/lib/schemas/auth";
import { login } from "@/lib/services/auth";

export default function LoginForm() {
  const { authenticate } = useAuth();
  const [processing, setProcessing] = useState(false);
  const [loggingIn, setLoggingIn] = useState(false);
  const [error, setError] = useState<ApiError | null>(null);

  const loginForm = useForm<LoginSchema>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: "",
      password: ""
    }
  });

  async function onLogin(values: LoginSchema) {
    setProcessing(true);
    setLoggingIn(false);

    const response = await login(values);

    if (!response.success) {
      setProcessing(false);
      setLoggingIn(false);
      setError(response.error ?? null);

      return;
    }

    setLoggingIn(true);
    setTimeout(() => {
      authenticate(response.value!.accessToken!);
      window.location.href = "/";
    }, 3000);
  }

  if (loggingIn) {
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
    <FormProvider {...loginForm}>
      <div className="flex flex-col gap-6">
        <Card>
          <CardHeader className="text-center">
            <CardTitle className="text-xl">Welcome back</CardTitle>
            <CardDescription>
              Login with your account
            </CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={loginForm.handleSubmit(onLogin)}>
              {error ? (
                <Alert variant="destructive">
                  <AlertCircleIcon />
                  <AlertTitle>Unable to sign you in</AlertTitle>
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
              <FormField control={loginForm.control} name="email" render={({ field }: {field: any }) => (
                <FormItem className="gap-0 py-4">
                  <FormLabel className="mb-2">Email</FormLabel>
                  <FormControl className="mb-2">
                    <Input type="text" disabled={processing} {...field} />
                  </FormControl>
                  <FormDescription />
                  <FormMessage />
                </FormItem>
              )} />
              <FormField control={loginForm.control} name="password" render={({ field }: {field: any }) => (
                <FormItem className="gap-0 py-4">
                  <FormLabel className="mb-2">Password</FormLabel>
                  <FormControl className="mb-2">
                    <Input type="password" disabled={processing} {...field} />
                  </FormControl>
                  <FormDescription />
                  <FormMessage className="mb-2" />
                </FormItem>
              )} />
              <Button type="submit" disabled={processing} className="w-full cursor-pointer">
                {processing ? (
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