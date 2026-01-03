export interface Project {
  id: string,
  name: string | null,
  createdAt: Date | null
}

export interface CreateProjectRequest {
  name: string | null;
}

export interface CreateProjectResponse {
  id: string;
  name: string;
}

export interface GetMyProjectsResponse {
  projects: GetMyProjectsProjectDto[]
}

export interface GetMyProjectsProjectDto {
  id: string;
  name: string | null;
  created_at: string | null;
}