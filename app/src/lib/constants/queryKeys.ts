export const QUERY_KEYS = {
  GET_MY_DETAILS: "user:me:details",
  GET_MY_PERMISSIONS: "user:me:permissions",
  GET_MY_PROJECTS: "user:me:projects",
  GET_PROJECT_BY_ID: "project",
  GET_USERS_IN_PROJECT: "project:users",
  GET_MY_PERMISSIONS_IN_PROJECT: "project:users:me:permissions",
  GET_ROLES_BY_SCOPE: "roles:scope"
} as const;