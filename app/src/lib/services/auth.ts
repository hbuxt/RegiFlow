import { LoginData, LoginResponse, SignupData, SignupResponse } from "../dtos/auth";
import { loginSchema, LoginSchema, signupSchema, SignupSchema } from "../schemas/auth";
import http, { HttpClientError } from "../utils/http";
import { errorResult, successResult, ValueResult } from "../utils/result";
import { toErrorMessages } from "../utils/zod";

export async function login(values: LoginSchema): Promise<ValueResult<LoginData>> {
  const validationResult = loginSchema.safeParse(values);
  
  if (!validationResult.success) {
    return errorResult({
      title: "Validation errors occurred",
      errors: toErrorMessages(validationResult.error)
    });
  }

  const formBody = new URLSearchParams();

  formBody.append('email', values.email);
  formBody.append('password', values.password);

  try {
    const response = await http.post<LoginResponse>({
      url: "/auth/login",
      contentType: "application/x-www-form-urlencoded",
      body: formBody.toString()
    });

    return successResult({
      accessToken: response.access_token
    });
  } catch (e) {
    console.error(e);

    if (e instanceof HttpClientError) {
      return errorResult({
        title: e.message,
        errors: e.data!
      });
    }

    throw e;
  }
}

export async function signup(values: SignupSchema): Promise<ValueResult<SignupData>> {
  const validationResult = signupSchema.safeParse(values);
  
  if (!validationResult.success) {
    return errorResult({
      title: "Validation errors occurred",
      errors: toErrorMessages(validationResult.error)
    });
  }

  const formBody = new URLSearchParams();

  formBody.append('email', values.email);
  formBody.append('password', values.password);
  formBody.append('confirm_password', values.confirmPassword);

  try {
    const response = await http.post<SignupResponse>({
      url: "/auth/register",
      contentType: "application/x-www-form-urlencoded",
      body: formBody.toString()
    });

    return successResult({
      accessToken: response.access_token
    });
  } catch (e) {
    console.error(e);

    if (e instanceof HttpClientError) {
      return errorResult({
        title: e.message,
        errors: e.data!
      });
    }

    throw e;
  }
}