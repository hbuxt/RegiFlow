import { errorResult, successResult, ValueResult } from "@/lib/shared/utils/result";
import { signupSchema, SignupSchema } from "./schema";
import { toErrorMessages } from "@/lib/shared/utils/zod";
import http, { HttpClientError } from "@/lib/shared/utils/http";
import { SignupData, SignupResponse } from "./dtos";

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