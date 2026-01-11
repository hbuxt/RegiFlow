export interface Project {
  id: string,
  name: string | null,
  description: string | null;
  createdAt: Date | null;
}

export interface ProjectUser {
  id: string;
  firstName: string | null;
  lastName: string | null;
  email: string;
  joinedAt: Date | null;
  roles: ProjectUserRole[];
}

export interface ProjectUserRole {
  id: string;
  name: string;
  assignedAt: Date | null;
}

export interface CreateProjectRequest {
  name: string | null;
  description: string | null;
}

export interface CreateProjectResponse {
  id: string;
  name: string;
}

export interface RenameProjectRequest {
  name: string;
}

export interface RenameProjectResponse {
  id: string;
}

export interface UpdateProjectDescriptionRequest {
  description: string;
}

export interface UpdateProjectDescriptionResponse {
  id: string;
}

export interface GetProjectByIdResponse {
  id: string;
  name: string;
  description: string;
  created_at: string;
}

export interface GetUsersInProjectResponse {
  users: GetUsersInProjectUserDto[]
}

export interface GetUsersInProjectUserDto {
  id: string;
  first_name: string | null;
  last_name: string | null;
  email: string;
  joined_at: string | null;
  roles: GetUsersInProjectRoleDto[]
}

export interface GetUsersInProjectRoleDto {
  id: string;
  name: string;
  assigned_at: string | null;
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

export interface InviteUserToProjectRequest {
  email: string;
  roles: string[];
}