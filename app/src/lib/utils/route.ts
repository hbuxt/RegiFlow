export function normalizeUrl(url?: string): string {
  let formattedUrl = url?.trim() || "/";

  // Prepend URL with a forward slash 
  if (!formattedUrl.startsWith("/")) {
    formattedUrl = "/" + formattedUrl;
  }

  // Remove trailing forward slash for non-root pages
  if (formattedUrl !== "/") {
    if (formattedUrl.endsWith("/")) {
      formattedUrl = formattedUrl.slice(0, -1);
    }
  }

  return formattedUrl;
}

export function isRouteActive(current: string, target?: string): boolean {
  const normalizedCurrent = normalizeUrl(current);
  const normalizedTarget = normalizeUrl(target);

  const encodedTarget = encodeURI(normalizedTarget);

  return normalizedCurrent === encodedTarget || normalizedCurrent.startsWith(encodedTarget + "/");
}

export function isExactRouteActive(current: string, target?: string): boolean {
  const normalizedCurrent = normalizeUrl(current);
  const normalizedTarget = normalizeUrl(target);

  const encodedTarget = encodeURI(normalizedTarget);

  return normalizedCurrent === encodedTarget;
}