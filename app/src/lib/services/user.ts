import { deleteAccountSchema, DeleteAccountSchema } from "../schemas/user";
import { GetMyDetailsResponse, GetMyPermissionsResponse, User } from "../types/user";
import http, { HttpClientError } from "../utils/http";
import { errorResult, Result, successResult, ValueResult } from "../utils/result";
import { toErrorMessages } from "../utils/zod";

export async function getMyDetails(): Promise<ValueResult<User>> {
  try {
    const response = await http.get<GetMyDetailsResponse>({
      url: "/users/me",
      contentType: "none"
    });

    return successResult({
      id: response.id,
      firstName: response.first_name,
      lastName: response.last_name,
      email: response.email
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

export async function deleteMyAccount(values: DeleteAccountSchema): Promise<Result> {
  const validationResult = deleteAccountSchema.safeParse(values);
    
  if (!validationResult.success) {
    return errorResult({
      title: "Validation errors occurred",
      errors: toErrorMessages(validationResult.error)
    });
  }

  try {
    await http.delete({
      url: "/users/me",
      contentType: "none"
    });

    return successResult();
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

export async function getMyPermissions(): Promise<ValueResult<string[]>> {
  try {
    const response = await http.get<GetMyPermissionsResponse>({
      url: "/users/me/permissions",
      contentType: "none"
    });

    return successResult(response.permissions);
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