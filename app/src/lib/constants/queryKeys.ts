export const QUERY_KEYS = {
  GET_MY_DETAILS: "user:me:details",
  GET_MY_PERMISSIONS: "user:me:permissions",
  GET_MY_PROJECTS: "user:me:projects",
  GET_PROJECT_BY_ID: (id: string) => `project:${id}` as const,
  GET_MY_PERMISSIONS_IN_PROJECT: (id: string) => `project:${id}:me:permissions`
} as const;