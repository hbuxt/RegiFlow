import SignUpForm from "@/components/SignupForm";
import { Layers2 } from "lucide-react";

export default function Signup() {
  return (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        <a href="/" className="flex items-center justify-center gap-2 self-center font-medium mb-4">
          <div className="bg-primary text-primary-foreground flex size-6 items-center justify-center rounded-md">
            <Layers2 className="size-4" />
          </div>
          RegiFlow
        </a>
        <SignUpForm />
      </div>
    </div>
  );
}