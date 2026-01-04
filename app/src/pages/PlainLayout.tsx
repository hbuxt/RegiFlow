import { getSession } from "@/lib/utils/session";
import { Outlet, redirect } from "react-router";

export function plainLayoutLoader() {
  const session = getSession();

  if (session) {
    throw redirect("/");
  }
}

export default function PlainLayout() {
  return <Outlet />;
}