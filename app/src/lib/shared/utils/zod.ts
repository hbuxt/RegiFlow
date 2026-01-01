import { ZodError } from "zod";
import { ApiErrorMessage } from "./result";

export function toErrorMessages(errors: ZodError<any>): ApiErrorMessage[] {
  return errors.issues.map(issue => ({
    code: issue.path.join("."),
    message: issue.message
  }));
}