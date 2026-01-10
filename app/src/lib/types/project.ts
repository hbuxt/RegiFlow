export interface Project {
  id: string,
  name: string | null,
  description: string | null;
  createdAt: Date | null
}

export interface CreateProjectRequest {
  name: string | null;
  description: string | null;
}

export interface CreateProjectResponse {
  id: string;
  name: string;
}

export interface GetProjectByIdResponse {
  id: string;
  name: string;
  description: string;
  created_at: string;
}

export interface GetMyProjectsResponse {
  projects: GetMyProjectsProjectDto[]
}

export interface GetMyProjectsProjectDto {
  id: string;
  name: string | null;
  created_at: string | null;
}

export interface GetMyPermissionsInProject {
  permissions: string[]
}