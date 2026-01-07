export interface AppErrorDetail {
  code: string;
  message: string;
}

export class AppError extends Error {
  readonly resource?: string;
  readonly status?: number;
  readonly details: AppErrorDetail[];

  constructor(params: {
    message: string;
    resource?: string;
    status?: number;
    details?: AppErrorDetail[];
  }) {
    super(params.message);

    this.resource = params.resource;
    this.status = params.status;
    this.details = params.details ?? [];
  }
}

export const appErrors = {
  badRequest: (resource?: string, details?: AppErrorDetail[]) => new AppError({
    message: "Bad request",
    status: 400,
    resource,
    details
  }),

  unauthorized: (resource?: string, details?: AppErrorDetail[]) => new AppError({
    message: "Unauthorized",
    status: 401,
    resource,
    details
  }),

  forbidden: (resource?: string, details?: AppErrorDetail[]) => new AppError({
    message: "Forbidden",
    status: 403,
    resource,
    details
  }),

  notFound: (resource?: string, details?: AppErrorDetail[]) => new AppError({
    message: "Not found",
    status: 404,
    resource,
    details
  }),

  conflict: (resource?: string, details?: AppErrorDetail[]) => new AppError({
    message: "Conflict",
    status: 409,
    resource,
    details
  }),

  rateLimit: (resource?: string, details?: AppErrorDetail[]) => new AppError({
    message: "Rate limit reached",
    status: 429,
    resource,
    details
  }),

  server: (resource?: string, details?: AppErrorDetail[]) => new AppError({
    message: "Server error has occurred",
    status: 500,
    resource,
    details
  }),

  network: (resource?: string, details?: AppErrorDetail[]) => new AppError({
    message: "Network error",
    status: 503,
    resource,
    details
  })
}