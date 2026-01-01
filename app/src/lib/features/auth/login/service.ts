import { errorResult, successResult, ValueResult } from "@/lib/shared/utils/result";
import { loginSchema, LoginSchema } from "./schema";
import { LoginData, LoginResponse } from "./dtos";
import { toErrorMessages } from "@/lib/shared/utils/zod";
import http, { HttpClientError } from "@/lib/shared/utils/http";

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