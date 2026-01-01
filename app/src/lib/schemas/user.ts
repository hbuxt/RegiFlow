import z from "zod";

export const deleteAccountSchema = z.object({
  id: z.string()
});

export type DeleteAccountSchema = z.infer<typeof deleteAccountSchema>;