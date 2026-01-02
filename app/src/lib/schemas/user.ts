import z from "zod";

export const updateMyDetailsSchema = z.object({
  firstName: z.string()
  .max(64, "First name exceeds the maximum length of 64 characters")
  .optional()
  .nullable(),

  lastName: z.string()
  .max(64, "Last name exceeds the maximum length of 64 characters")
  .optional()
  .nullable()
});

export const deleteMyAccountSchema = z.object({
  id: z.string()
});

export type UpdateMyDetailsSchema = z.infer<typeof updateMyDetailsSchema>;
export type DeleteMyAccountSchema = z.infer<typeof deleteMyAccountSchema>;