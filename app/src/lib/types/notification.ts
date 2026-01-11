import { Project } from "./project";
import { Role } from "./role";
import { User } from "./user";

export interface Notification {
  id: string;
  type: string;
  status: string;
  createdAt: Date | null;
  invitation: InvitationDetails | null;
}

export interface InvitationDetails {
  sentBy: User,
  regarding: Project,
  roles: Role[],
  token: string;
  expiresAt: Date | null;
}

export interface GetMyNotificationsResponse {
  notifications: GetMyNotificationsNotificationDto[];
}

export interface GetMyNotificationsNotificationDto {
  id: string;
  type: string;
  status: string;
  created_at: string | null;
  invitation: GetMyNotificationsInvitationDetailsDto | null;
}

export interface GetMyNotificationsInvitationDetailsDto {
  sent_by: GetMyNotificationsInvitationUserDto;
  regarding: GetMyNotificationsInvitationProjectDto;
  roles: GetMyNotificationsInvitationRoleDto[];
  token: string;
  expires_at: string | null;
}

export interface GetMyNotificationsInvitationProjectDto {
  id: string;
  name: string;
  description : string | null;
}

export interface GetMyNotificationsInvitationUserDto {
  id: string;
  email: string;
}

export interface GetMyNotificationsInvitationRoleDto {
  id: string;
  name: string;
}