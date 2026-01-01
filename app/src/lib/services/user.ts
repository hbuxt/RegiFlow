import { GetMyDetailsResponse, User } from "../types/user";
import http, { HttpClientError } from "../utils/http";
import { errorResult, successResult, ValueResult } from "../utils/result";

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