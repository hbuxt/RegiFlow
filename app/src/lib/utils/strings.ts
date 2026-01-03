export function ellipsizable(input: string | null, maxlength: number = 15): boolean {
  if (!input) {
    return false;
  }

  if (input.length > maxlength) {
    return true;
  }

  return false;
}

export function ellipsize(input: string | null, maxLength: number = 15): string {
  if (!input) {
    return "";
  }

  if (input.length > maxLength) {
    return input.slice(0, maxLength) + "...";
  }

  return input;
}