import z from "zod";

export const createProjectSchema = z.object({
  name: z.string()
    .min(4, "Name must be at least 4 characters long")
    .max(64, "Name exceeds the maximum length of 64 characters"),

  description: z.string()
    .max(256, "Description exceeds the maximum length of 256 characters")
});

export const renameProjectSchema = z.object({
  id: z.string(),
  name: z.string()
    .min(4, "Name must be at least 4 characters long")
    .max(64, 'Name is too long, it must not exceed 64 characters')
});

export const updateProjectDescriptionSchema = z.object({
  id: z.string(),
  description: z.string()
    .max(256, "Description is too long, it must not exceed 256 characters")
    .optional()
})

export const deleteProjectSchema = z.object({
  id: z.string()
});

export const inviteUserToProjectSchema = z.object({
  id: z.string(),
  email: z.string()
      .min(1, 'Email is required')
      .email('Invalid email address')
      .max(256, 'Email is too long, it must not exceed 256 characters'),
  role: z.string()
    .min(1, 'Role is required')
});

export type RenameProjectSchema = z.infer<typeof renameProjectSchema>;
export type UpdateProjectDescriptionSchema = z.infer<typeof updateProjectDescriptionSchema>;
export type DeleteProjectSchema = z.infer<typeof deleteProjectSchema>;
export type CreateProjectSchema = z.infer<typeof createProjectSchema>;
export type InviteUserToProjectSchema = z.infer<typeof inviteUserToProjectSchema>;