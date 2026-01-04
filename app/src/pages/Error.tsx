import { Empty, EmptyDescription, EmptyHeader, EmptyMedia, EmptyTitle } from "@/components/ui/empty";
import { ApiErrorMessage } from "@/lib/utils/result";
import { Ban } from "lucide-react";

interface ErrorProps {
  title: string;
  errors: ApiErrorMessage[] | null;
}

export default function Error(props: ErrorProps) {
  return (
    <Empty>
      <EmptyHeader>
        <EmptyMedia variant="icon">
          <Ban />
        </EmptyMedia>
        <EmptyTitle>{props.title}</EmptyTitle>
          {props.errors && props.errors.length > 0 ? (
            <EmptyDescription>
              {props.errors.length == 1 ? (
                <>{props.errors[0].message}</>
              ) : (
                <ul className="list-inside list-disc text-sm">
                  {props.errors.map((error, index) => (
                    <li key={index}>{error.message}</li>
                  ))}
                </ul>
              )} 
            </EmptyDescription>
          ) : null}
      </EmptyHeader>

    </Empty>
  );
}