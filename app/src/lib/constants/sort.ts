export const SORT_BY_AZ = "A-Z";
export const SORT_BY_ZA = "Z-A";
export const SORT_BY_MOST_RECENT = "Most Recent";
export const SORT_BY_OLDEST = "Oldest";

export const SORT_BY = [
  SORT_BY_AZ,
  SORT_BY_ZA,
  SORT_BY_MOST_RECENT,
  SORT_BY_OLDEST,
] as const;

export type SortBy = typeof SORT_BY[number];