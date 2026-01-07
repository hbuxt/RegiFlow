export interface Session {
  id: string;
  token: Token;
  rawToken: string;
}

export interface Token {
  sub: string;
  aud: string;
  iss: string;
  exp: number;
  iat: number;
  nbf: number;
}

export interface LoginResponse {
  access_token: string;
}

export interface SignupResponse {
  access_token: string | null;
}