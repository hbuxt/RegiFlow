import { GetRolesByScopeResponse, Role } from "../types/role";
import http from "../utils/http";

export async function getRolesByScope(scope: string): Promise<Role[]> {
  const params = new URLSearchParams({ scope });

  const response = await http.get<GetRolesByScopeResponse>({
    url: `/roles?${params.toString()}`,
    contentType: "none"
  });

  const roles = response.roles.map((roleDto) => {
    const role: Role = {
      id: roleDto.id,
      name: roleDto.name
    };

    return role;
  });

  return roles;
}