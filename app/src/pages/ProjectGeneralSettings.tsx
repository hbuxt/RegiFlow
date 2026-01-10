import DeleteProjectForm from "@/components/DeleteProjectForm";
import RenameProjectForm from "@/components/RenameProjectForm";
import UpdateProjectDescriptionForm from "@/components/UpdateProjectDescriptionForm";
import { Project } from "@/lib/types/project";
import { useOutletContext } from "react-router";

export default function ProjectGeneralSettings() {
  const { project, permissions } = useOutletContext() as { project: Project, permissions: string[] };
  
  return (
    <div className="flex flex-col gap-4">
      <div>
        <div className="w-full border-b pb-4">
          <h2 className="text-lg font-normal">General</h2>
        </div>
      </div>
      <RenameProjectForm project={project} permissions={permissions} />
      <UpdateProjectDescriptionForm project={project} permissions={permissions} />
      <DeleteProjectForm project={project} permissions={permissions} />
    </div>
  );
}