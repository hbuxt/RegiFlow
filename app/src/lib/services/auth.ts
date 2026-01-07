import { loginSchema, LoginSchema, signupSchema, SignupSchema } from "../schemas/auth";
import { LoginResponse, SignupResponse } from "../types/auth";
import { appErrors } from "../utils/errors";
import http from "../utils/http";
import { toErrorDetails } from "../utils/zod";

export async function login(values: LoginSchema): Promise<string> {
  const validationResult = loginSchema.safeParse(values);
  
  if (!validationResult.success) {
    throw appErrors.badRequest("Login", toErrorDetails(validationResult.error));
  }

  const formBody = new URLSearchParams();

  formBody.append('email', values.email);
  formBody.append('password', values.password);

  const response = await http.post<LoginResponse>({
    url: "/auth/login",
    contentType: "application/x-www-form-urlencoded",
    body: formBody.toString()
  });

  const session = response.access_token;
  return session;
}

export async function signup(values: SignupSchema): Promise<string | null> {
  const validationResult = signupSchema.safeParse(values);
  
  if (!validationResult.success) {
    throw appErrors.badRequest("Signup", toErrorDetails(validationResult.error));
  }

  const formBody = new URLSearchParams();

  formBody.append('email', values.email);
  formBody.append('password', values.password);
  formBody.append('confirm_password', values.confirmPassword);

  const response = await http.post<SignupResponse>({
    url: "/auth/register",
    contentType: "application/x-www-form-urlencoded",
    body: formBody.toString()
  });

  const session = response.access_token;
  return session;
}