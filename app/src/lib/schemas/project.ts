import z from "zod";

export const createProjectSchema = z.object({
  name: z.string()
    .min(4, "Name must be at least 4 characters long")
    .max(64, "Name exceeds the maximum length of 64 characters"),

  description: z.string()
    .max(256, "Description exceeds the maximum length of 256 characters")
});

export type CreateProjectSchema = z.infer<typeof createProjectSchema>;