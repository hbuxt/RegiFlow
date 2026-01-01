import { ArrowLeftToLine, ArrowRightToLine } from "lucide-react";
import { Button } from "./ui/button";
import { useSidebar } from "./ui/sidebar";
import { Tooltip, TooltipContent, TooltipTrigger } from "./ui/tooltip";

export default function AppSidebarTrigger() {
  const { open, isMobile, toggleSidebar } = useSidebar();

  return (
    <Tooltip delayDuration={800}>
      <TooltipTrigger asChild>
        <Button variant="outline" size="icon" aria-label={open ? "Close" : "Open"} onClick={toggleSidebar} className="cursor-pointer">
            {isMobile ? <ArrowRightToLine /> : open ? <ArrowLeftToLine /> : <ArrowRightToLine /> }
        </Button>
      </TooltipTrigger>
      <TooltipContent>
        <span>{isMobile ? "Open sidebar" : open ? "Close sidebar" : "Open sidebar"}</span>
      </TooltipContent>
    </Tooltip>
  );
}