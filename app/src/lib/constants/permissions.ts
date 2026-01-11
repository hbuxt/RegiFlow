export const PERMISSIONS = {
  USER_PROFILE_UPDATE: "user.profile.update",
  USER_EMAIL_UPDATE: "user.email.update",
  USER_PASSWORD_UPDATE: "user.password.update",
  USER_DELETE: "user.delete",

  PROJECT_READ: "project.read",
  PROJECT_CREATE: "project.create",
  PROJECT_NAME_UPDATE: "project.name.update",
  PROJECT_DESCRIPTION_UPDATE: "project.description.update",
  PROJECT_DELETE: "project.delete",
  PROJECT_USERS_READ: "project.users.read",
  PROJECT_INVITATIONS_CREATE: "project.invitations.create"
} as const;