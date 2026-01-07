import { getBaseApiUrl } from "@/lib/utils/env";
import { getSession } from "./session";
import { AppErrorDetail, appErrors } from "./errors";

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

    if (response.status === 204) {
      return null as T;
    }

    const json = await response.json();

    if (response.ok) {
      return json as T;
    }

    const details = this.toErrorDetails(json);

    if (response.status == 400) {
      throw appErrors.badRequest(url, details);
    } else if (response.status == 401) {
      throw appErrors.unauthorized(url, details);
    } else if (response.status == 403) {
      throw appErrors.forbidden(url, details);
    } else if (response.status == 404) {
      throw appErrors.notFound(url, details);
    } else if (response.status == 409) {
      throw appErrors.conflict(url, details);
    } else if (response.status == 429) {
      throw appErrors.rateLimit(url, details);
    } else {
      throw appErrors.server(url, details);
    }
  }

  private toErrorDetails(data: any): AppErrorDetail[] {
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

    let response: Response;

    try {
      response = await fetch(request);
    } catch (e) {
      throw appErrors.network(request.url, [{ 
        code: "network_error", 
        message: "Network error. Please check your internet connection." 
      }]);
    }

    for (const handler of this.afterHandlers) {
      response = await handler(response, request) || response;
    }

    return response;
  }
}

const pipeline = new HttpClientPipeline();
const options: HttpClientOptions = {
  baseAddress: getBaseApiUrl()
};

pipeline.useBefore((request) => {
  const session = getSession();

  if (session?.rawToken) {
    request.headers.set("Authorization", `Bearer ${session.rawToken}`);
  }

  return request;
});

pipeline.useBefore(async (request) => {
  return request;
});

export default new HttpClient(options, pipeline);