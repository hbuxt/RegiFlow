import z from "zod";

export const deleteMyAccountSchema = z.object({
  id: z.string()
});

export type DeleteMyAccountSchema = z.infer<typeof deleteMyAccountSchema>;