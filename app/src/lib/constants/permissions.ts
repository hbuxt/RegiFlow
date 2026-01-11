export const PERMISSIONS = {
  USER_DELETE: "user.delete",
  USER_PROFILE_UPDATE: "user.profile.update",
  USER_EMAIL_UPDATE: "user.email.update",
  USER_PASSWORD_UPDATE: "user.password.update",

  PROJECT_CREATE: "project.create",
  PROJECT_RENAME: "project.name.update",
  PROJECT_UPDATE: "project.description.update",
  PROJECT_DELETE: "project.delete",
  PROJECT_LEAVE: "project.users.leave",
  PROJECT_READ: "project.read",
  PROJECT_USERS_READ: "project.users.read",
  PROJECT_USERS_INVITE: "project.invitations.create"
} as const;