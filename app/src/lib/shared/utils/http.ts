import { getBaseApiUrl } from "@/lib/shared/utils/env";
import { ApiErrorMessage } from "@/lib/shared/utils/result";

export interface HttpClientOptions {
  baseAddress: string;
}

export interface HttpClientMessage {
  url: string;
  contentType: string;
  body?: string;
}

class HttpClient {
  private options: HttpClientOptions;
  private pipeline: HttpClientPipeline;

  constructor(options: HttpClientOptions, pipeline: HttpClientPipeline) {
    this.options = options;
    this.pipeline = pipeline;
  }

  public async get<T>(message: HttpClientMessage): Promise<T> {
    return await this.send<T>("GET", message);
  }

  public async put<T>(message: HttpClientMessage): Promise<T> {
    return await this.send<T>("PUT", message);
  }

  public async post<T>(message: HttpClientMessage): Promise<T> {
    return await this.send<T>("POST", message);
  }

  public async delete<T>(message: HttpClientMessage): Promise<T> {
    return await this.send<T>("DELETE", message);
  }

  protected async send<T>(method: string, message: HttpClientMessage): Promise<T> {
    const url = this.options.baseAddress + message.url;
    const headers: Record<string, string> = {
      "Content-Type": message.contentType
    };

    const request: RequestInit = {
      method: method,
      body: message.body,
      headers: headers
    };

    const response = await this.pipeline.execute(new Request(url, request));
    const json = await response.json();

    if (response.ok) {
      return json as T;
    }

    const errors = this.toErrorMessages(json);

    if (response.status == 400) {
      throw new BadRequestError(url, response.status, errors);
    } else if (response.status == 401) {
      throw new UnauthorizedError(url, response.status, errors);
    } else if (response.status == 403) {
      throw new ForbiddenError(url, response.status, errors);
    } else if (response.status == 404) {
      throw new NotFoundError(url, response.status, errors);
    } else if (response.status == 409) {
      throw new ConflictError(url, response.status, errors);
    } else if (response.status == 429) {
      throw new RateLimitError(url, response.status, errors);
    } else {
      throw new ServerError(url, response.status, errors);
    }
  }

  private toErrorMessages(data: any): ApiErrorMessage[] {
    if (!data) {
      return [];
    }

    if (!data.errors || data.errors.length === 0) {
      return [{
        code: data.title,
        message: data.detail
      }];
    }

    try {
      return data.errors.map((error: any) => ({
        code: error.error_code,
        message: error.error_message,
      }));
    } catch (e) {
      console.error(e);
      return [{
        code: data.title,
        message: data.detail
      }];
    }
  }
}

class HttpClientPipeline {
  private beforeHandlers: Array<(request: Request) => Promise<Request> | Request>;
  private afterHandlers: Array<(response: Response, request: Request) => Promise<Response> | Response>;

  constructor() {
    this.beforeHandlers = [];
    this.afterHandlers = [];
  }

  useBefore(fn: (request: Request) => Promise<Request> | Request) {
    this.beforeHandlers.push(fn);
  }

  useAfter(fn: (response: Response, request: Request) => Promise<Response> | Response) {
    this.afterHandlers.push(fn);
  }

  async execute(input: Request): Promise<Response> {
    let request = input;

    for (const handler of this.beforeHandlers) {
      request = await handler(request) || request;
    }

    let response = await fetch(request);

    for (const handler of this.afterHandlers) {
      response = await handler(response, request) || response;
    }

    return response;
  }
}

export abstract class HttpClientError extends Error {
  public resource: string;
  public status: number;
  public data?: ApiErrorMessage[];

  constructor(message: string, resource: string, status: number, data?: ApiErrorMessage[]) {
    super(message);
    
    this.resource = resource;
    this.status = status;
    this.data = data;
  }
}

export class BadRequestError extends HttpClientError {
  constructor(resource: string, status: number, data?: ApiErrorMessage[]) {
    super("Oops, looks like you missed something", resource, status, data);
  }
}

export class UnauthorizedError extends HttpClientError {
  constructor(resource: string, status: number, data?: ApiErrorMessage[]) {
    super("Oops, you need to re-authenticate", resource, status, data);
  }
}

export class ForbiddenError extends HttpClientError {
  constructor(resource: string, status: number, data?: ApiErrorMessage[]) {
    super("Oops, you don't have permissions", resource, status, data);
  }
}

export class NotFoundError extends HttpClientError {
  constructor(resource: string, status: number, data?: ApiErrorMessage[]) {
    super("Oops, you should check your data", resource, status, data);
  }
}

export class ConflictError extends HttpClientError {
  constructor(resource: string, status: number, data?: ApiErrorMessage[]) {
    super("Oops, you should check your data", resource, status, data);
  }
}

export class RateLimitError extends HttpClientError {
  constructor(resource: string, status: number, data?: ApiErrorMessage[]) {
    super("Oops, steady on there!", resource, status, data);
  }
}

export class ServerError extends HttpClientError {
  constructor(resource: string, status: number, data?: ApiErrorMessage[]) {
    super("Oops, something went wrong!", resource, status, data);
  }
}

const pipeline = new HttpClientPipeline();
const options: HttpClientOptions = {
  baseAddress: getBaseApiUrl()
};

pipeline.useBefore((request) => {
  return request;
});

pipeline.useBefore(async (request) => {
  return request;
});

export default new HttpClient(options, pipeline);