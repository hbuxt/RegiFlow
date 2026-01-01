import z from "zod";

export const signupSchema = z.object({
  email: z.string()
    .min(1, 'Email is required')
    .email('Invalid email address')
    .max(256, 'Email is too long, it must not exceed 256 characters'),

  password: z.string()
    .min(8, 'Password is too short, it must be at least 8 characters')
    .max(64, 'Password is too long, it must not exceed 64 characters')
    .regex(/[A-Z]/, 'Password must include an uppercase letter')
    .regex(/[a-z]/, 'Password must include a lowercase letter')
    .regex(/[0-9]/, 'Password must include a number')
    .regex(/[^A-Za-z0-9]/, 'Password must include a special character'),

  confirmPassword: z.string()
    .min(1, 'Confirm password is required')
}).refine((data) => data.password === data.confirmPassword, {
  message: 'Passwords do not match',
  path: ['confirmPassword']
});

export type SignupSchema = z.infer<typeof signupSchema>;