export const sortByAz = "A-Z";
export const sortByZa = "Z-A";
export const sortByMostRecent = "Most Recent";
export const sortByOldest = "Oldest";
export const sortBy = [
  sortByAz,
  sortByZa,
  sortByMostRecent,
  sortByOldest
];

export type SortBy = typeof sortBy[number];