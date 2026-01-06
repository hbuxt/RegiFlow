import { deleteMyAccountSchema, DeleteMyAccountSchema, updateMyDetailsSchema, UpdateMyDetailsSchema } from "../schemas/user";
import { GetMyDetailsResponse, GetMyPermissionsResponse, UpdateMyDetailsRequest, UpdateMyDetailsResponse, User } from "../types/user";
import http, { HttpClientError } from "../utils/http";
import { errorResult, Result, successResult } from "../utils/result";
import { toErrorMessages } from "../utils/zod";

export async function getMyDetails(): Promise<User> {
  const response = await http.get<GetMyDetailsResponse>({
    url: "/users/me",
    contentType: "none"
  });

  return {
    id: response.id,
    firstName: response.first_name,
    lastName: response.last_name,
    email: response.email
  };
}

export async function updateMyDetails(values: UpdateMyDetailsSchema): Promise<Result> {
  const validationResult = updateMyDetailsSchema.safeParse(values);
    
  if (!validationResult.success) {
    return errorResult({
      title: "Validation errors occurred",
      errors: toErrorMessages(validationResult.error)
    });
  }

  try {
    const request: UpdateMyDetailsRequest = {
      first_name: values.firstName ?? "",
      last_name: values.lastName ?? ""
    };

    await http.put<UpdateMyDetailsResponse>({
      url: "/users/me",
      body: JSON.stringify(request),
      contentType: "application/json"
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

export async function deleteMyAccount(values: DeleteMyAccountSchema): Promise<Result> {
  const validationResult = deleteMyAccountSchema.safeParse(values);
    
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

export async function getMyPermissions(): Promise<string[]> {
  const response = await http.get<GetMyPermissionsResponse>({
    url: "/users/me/permissions",
    contentType: "none"
  });

  return response.permissions;
}