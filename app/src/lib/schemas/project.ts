import z from "zod";

export const createProjectSchema = z.object({
  name: z.string()
  .min(4, "Project name must be at least 4 characters long")
  .max(64, "Project name exceeds the maximum length of 64 characters")
});

export type CreateProjectSchema = z.infer<typeof createProjectSchema>;