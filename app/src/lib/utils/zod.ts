import { ZodError } from "zod";
import { AppErrorDetail } from "./errors";

export function toErrorDetails(errors: ZodError<any>): AppErrorDetail[] {
  return errors.issues.map(issue => ({
    code: issue.path.join("."),
    message: issue.message
  }));
}