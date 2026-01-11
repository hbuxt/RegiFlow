import { SORT_BY_AZ, SORT_BY_MOST_RECENT, SORT_BY_OLDEST, SORT_BY_ZA, SortBy } from "../constants/sort";
import { createProjectSchema, CreateProjectSchema, deleteProjectSchema, DeleteProjectSchema, InviteUserToProjectSchema, renameProjectSchema, RenameProjectSchema, updateProjectDescriptionSchema, UpdateProjectDescriptionSchema } from "../schemas/project";
import { CreateProjectRequest, CreateProjectResponse, GetMyPermissionsInProject, GetMyProjectsResponse, GetProjectByIdResponse, GetUsersInProjectResponse, InviteUserToProjectRequest, Project, ProjectUser, ProjectUserRole, RenameProjectRequest, RenameProjectResponse, UpdateProjectDescriptionRequest, UpdateProjectDescriptionResponse } from "../types/project";
import { UpdateMyDetailsRequest } from "../types/user";
import { appErrors } from "../utils/errors";
import http from "../utils/http";
import { toErrorDetails } from "../utils/zod";

export async function createProject(values: CreateProjectSchema): Promise<Project> {
  const validationResult = createProjectSchema.safeParse(values);
      
  if (!validationResult.success) {
    throw appErrors.badRequest("Create project", toErrorDetails(validationResult.error));
  }

  const request: CreateProjectRequest = {
    name: values.name,
    description: values.description
  };

  const response = await http.post<CreateProjectResponse>({
    url: "/projects",
    body: JSON.stringify(request),
    contentType: "application/json"
  });

  return {
    id: response.id,
    name: response.name,
    description: null,
    createdAt: null
  };
}

export async function renameProject(values: RenameProjectSchema): Promise<undefined> {
  const validationResult = renameProjectSchema.safeParse(values);
      
  if (!validationResult.success) {
    throw appErrors.badRequest("Rename project", toErrorDetails(validationResult.error));
  }

  const request: RenameProjectRequest = {
    name: values.name
  };

  await http.put<RenameProjectResponse>({
    url: `/projects/${values.id}/rename`,
    body: JSON.stringify(request),
    contentType: "application/json"
  });
}

export async function updateProjectDescription(values: UpdateProjectDescriptionSchema): Promise<undefined> {
  const validationResult = updateProjectDescriptionSchema.safeParse(values);

  if (!validationResult.success) {
    throw appErrors.badRequest("Update project", toErrorDetails(validationResult.error));
  }

  const request: UpdateProjectDescriptionRequest = {
    description: values.description ?? ""
  };

  await http.put<UpdateProjectDescriptionResponse>({
    url: `/projects/${values.id}`,
    body: JSON.stringify(request),
    contentType: "application/json"
  });
}

export async function deleteProjectById(values: DeleteProjectSchema): Promise<undefined> {
  const validationResult = deleteProjectSchema.safeParse(values);

  if (!validationResult.success) {
    throw appErrors.badRequest("Delete project", toErrorDetails(validationResult.error));
  }

  await http.delete({
    url: `/projects/${values.id}`,
    contentType: "none"
  });
}

export async function getProjectById(id: string) : Promise<Project> {
  const response = await http.get<GetProjectByIdResponse>({
    url: `/projects/${id}`,
    contentType: "none"
  });

  const project: Project = {
    id: response.id,
    name: response.name,
    description: response.description,
    createdAt: response.created_at ? new Date(response.created_at) : null
  };

  return project;
}

export async function getUsersInProject(id: string): Promise<ProjectUser[]> {
  const response = await http.get<GetUsersInProjectResponse>({
    url: `/projects/${id}/users`,
    contentType: "none"
  });

  const users = response.users.map((userDto) => {
    const roles = userDto.roles.map((roleDto) => {
      const role: ProjectUserRole = {
        id: roleDto.id,
        name: roleDto.name,
        assignedAt: roleDto.assigned_at ? new Date(roleDto.assigned_at) : null
      };

      return role;
    });

    const user: ProjectUser = {
      id: userDto.id,
      firstName: userDto.first_name,
      lastName: userDto.last_name,
      email: userDto.email,
      joinedAt: userDto.joined_at ? new Date(userDto.joined_at) : null,
      roles: roles
    };

    return user;
  });

  return users;
}

export async function getMyProjects(): Promise<Project[]> {
  const response = await http.get<GetMyProjectsResponse>({
    url: "/users/me/projects",
    contentType: "none"
  });

  const projects = response.projects.map((dto) => {
    const project: Project = {
      id: dto.id,
      name: dto.name,
      description: null,
      createdAt: dto.created_at ? new Date(dto.created_at) : null
    };

    return project;
  })

  return projects;
}

export async function getMyPermissionsInProject(id: string): Promise<string[]> {
  const response = await http.get<GetMyPermissionsInProject>({
    url: `/projects/${id}/users/me/permissions`,
    contentType: "none"
  });

  return response.permissions;
}

export async function inviteUserToProject(values: InviteUserToProjectSchema): Promise<undefined> {
  const request: InviteUserToProjectRequest = {
    email: values.email,
    roles: [values.role]
  };

  await http.post({
    url: `/projects/${values.id}/invitations`,
    body: JSON.stringify(request),
    contentType: "application/json"
  });
}

export function sortProjects(projects: Project[], sortBy: SortBy): Project[] {
  const sortedProjects = [...projects];

  switch (sortBy) {
    case SORT_BY_AZ:
      return sortedProjects.sort((a, b) => {
        if (a.name && b.name) {
          return a.name.localeCompare(b.name);
        }

        if (!a.name && b.name) {
           // If a.name is missing, push it to the end
          return 1;
        }

        if (a.name && !b.name) {
          // If b.name is missing, push it to the end
          return -1;
        }

        // Both are undefined, consider them equal
        return 0;
      });

    case SORT_BY_ZA:
      return sortedProjects.sort((a, b) => {
        if (a.name && b.name) {
          return b.name.localeCompare(a.name);
        }

        if (!a.name && b.name) {
           // If a.name is missing, push it to the end
          return 1;
        }

        if (a.name && !b.name) {
          // If b.name is missing, push it to the end
          return -1;
        }

        // Both are undefined, consider them equal
        return 0;
      });

    case SORT_BY_MOST_RECENT:
      return sortedProjects.sort((a, b) => {
        // Handle nullable createdAt dates
        const aDate = a.createdAt || new Date(0); // Use a very early date if undefined
        const bDate = b.createdAt || new Date(0); // Use a very early date if undefined
        return bDate.getTime() - aDate.getTime();
      });

    case SORT_BY_OLDEST:
      return sortedProjects.sort((a, b) => {
        const aDate = a.createdAt || new Date(0); // Use a very early date if undefined
        const bDate = b.createdAt || new Date(0); // Use a very early date if undefined
        return aDate.getTime() - bDate.getTime();
      });

    default:
      return sortedProjects; // No sorting
  }
}