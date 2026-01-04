import { SORT_BY_AZ, SORT_BY_MOST_RECENT, SORT_BY_OLDEST, SORT_BY_ZA, SortBy } from "../constants/sort";
import { createProjectSchema, CreateProjectSchema } from "../schemas/project";
import { CreateProjectRequest, CreateProjectResponse, GetMyProjectsResponse, Project } from "../types/project";
import http, { HttpClientError } from "../utils/http";
import { errorResult, successResult, ValueResult } from "../utils/result";
import { toErrorMessages } from "../utils/zod";

export async function createProject(values: CreateProjectSchema): Promise<ValueResult<Project>> {
  const validationResult = createProjectSchema.safeParse(values);
      
  if (!validationResult.success) {
    return errorResult({
      title: "Validation errors occurred",
      errors: toErrorMessages(validationResult.error)
    });
  }

  try {
    const request: CreateProjectRequest = {
      name: values.name,
      description: values.description
    };

    const response = await http.post<CreateProjectResponse>({
      url: "/projects",
      body: JSON.stringify(request),
      contentType: "application/json"
    });

    return successResult({
      id: response.id,
      name: response.name,
      createdAt: null
    });
  } catch (e) {
    if (e instanceof HttpClientError) {
      return errorResult({
        title: e.message,
        errors: e.data!
      });
    }

    throw e;
  }
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
      createdAt: dto.created_at ? new Date(dto.created_at) : null
    };

    return project;
  })

  return projects;
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