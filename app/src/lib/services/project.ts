import { SortBy, sortByAz, sortByMostRecent, sortByOldest, sortByZa } from "../schemas/common";
import { Project } from "../types/project";

export function sortProjects(projects: Project[], sortBy: SortBy): Project[] {
  const sortedProjects = [...projects];

  switch (sortBy) {
    case sortByAz:
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

    case sortByZa:
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

    case sortByMostRecent:
      return sortedProjects.sort((a, b) => {
        // Handle nullable createdAt dates
        const aDate = a.createdAt || new Date(0); // Use a very early date if undefined
        const bDate = b.createdAt || new Date(0); // Use a very early date if undefined
        return bDate.getTime() - aDate.getTime();
      });

    case sortByOldest:
      return sortedProjects.sort((a, b) => {
        const aDate = a.createdAt || new Date(0); // Use a very early date if undefined
        const bDate = b.createdAt || new Date(0); // Use a very early date if undefined
        return aDate.getTime() - bDate.getTime();
      });

    default:
      return sortedProjects; // No sorting
  }
}