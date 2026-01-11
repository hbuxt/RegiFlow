export interface Role {
  id: string;
  name: string;
}

export interface GetRolesByScopeResponse {
  roles: GetRolesByScopeRoleDto[];
}

export interface GetRolesByScopeRoleDto {
  id: string;
  name: string;
}