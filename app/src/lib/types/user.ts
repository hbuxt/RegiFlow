export interface User {
  id: string;
  firstName: string | null;
  lastName: string | null;
  email: string;
}

export interface GetMyDetailsResponse {
  id: string;
  first_name: string | null;
  last_name : string | null;
  email: string;
}

export interface GetMyPermissionsResponse {
  permissions: string[]
}