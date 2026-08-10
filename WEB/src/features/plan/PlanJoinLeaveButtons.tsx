import {
  useIsPlanParticipated,
  useJoinPlan,
  useLeavePlan,
} from "../../lib/hooks/usePlan";

import { Box, Button, CircularProgress } from "@mui/material";

interface PlanJoinLeaveButtonsProps {
  planId: string;
}

export default function PlanJoinLeaveButtons({planId}: PlanJoinLeaveButtonsProps) {
 
  const { data: participated, isLoading } = useIsPlanParticipated(planId);

  const joinPlan = useJoinPlan(planId);
  const leavePlan = useLeavePlan(planId);

   if (isLoading) {
     return (
       <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
         <CircularProgress />
       </Box>
     );
   }

  if (participated) {
    return (
      <Button
        variant="outlined"
        color="error"
        onClick={() => leavePlan.mutate()}
        disabled={leavePlan.isPending}
        sx={{ width: "100%" }}
      >
        {leavePlan.isPending ? "Leaving..." : "Leave"}
      </Button>
    );
  }

  return (
    <Button
      variant="contained"
      color="primary"
      onClick={() => joinPlan.mutate()}
      disabled={joinPlan.isPending}
      sx={{width:"100%"}}
    >
      {joinPlan.isPending ? "Joining..." : "Join"}
    </Button>
  );
}
