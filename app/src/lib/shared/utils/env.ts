export function getBaseApiUrl(): string {
  return import.meta.env.VITE_API_URL ?? "https://localhost:7283/api";
}
