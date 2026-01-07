import { deleteMyAccountSchema, DeleteMyAccountSchema, updateMyDetailsSchema, UpdateMyDetailsSchema } from "../schemas/user";
import { GetMyDetailsResponse, GetMyPermissionsResponse, UpdateMyDetailsRequest, UpdateMyDetailsResponse, User } from "../types/user";
import { appErrors } from "../utils/errors";
import http from "../utils/http";
import { toErrorDetails } from "../utils/zod";

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

export async function updateMyDetails(values: UpdateMyDetailsSchema): Promise<undefined> {
  const validationResult = updateMyDetailsSchema.safeParse(values);
    
  if (!validationResult.success) {
    throw appErrors.badRequest("Update my details", toErrorDetails(validationResult.error));
  }

  const request: UpdateMyDetailsRequest = {
    first_name: values.firstName ?? "",
    last_name: values.lastName ?? ""
  };

  await http.put<UpdateMyDetailsResponse>({
    url: "/users/me",
    body: JSON.stringify(request),
    contentType: "application/json"
  });
}

export async function deleteMyAccount(values: DeleteMyAccountSchema): Promise<undefined> {
  const validationResult = deleteMyAccountSchema.safeParse(values);
    
  if (!validationResult.success) {
    throw appErrors.badRequest("Delete my account", toErrorDetails(validationResult.error));
  }

  await http.delete({
    url: "/users/me",
    contentType: "none"
  });
}

export async function getMyPermissions(): Promise<string[]> {
  const response = await http.get<GetMyPermissionsResponse>({
    url: "/users/me/permissions",
    contentType: "none"
  });

  return response.permissions;
}