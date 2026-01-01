export interface Result {
  success: boolean;
  error?: ApiError;
}

export interface ValueResult<T> extends Result {
  value?: T;
}

export interface ApiError {
  title: string,
  errors: ApiErrorMessage[]
}

export interface ApiErrorMessage {
  code: string,
  message: string
}

export function successResult<T = unknown>(value?: T): ValueResult<T> | Result {
  return { 
    success: true, 
    value: value 
  };
}

export function errorResult<T = unknown>(error: ApiError): ValueResult<T> | Result {
  return { 
    success: false, 
    error: error
  };
}