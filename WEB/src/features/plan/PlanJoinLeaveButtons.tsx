import { useState } from "react";
import {
  useIsPlanParticipated,
  useJoinPlan,
  useLeavePlan,
} from "../../lib/hooks/usePlan";

import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
} from "@mui/material";

interface PlanJoinLeaveButtonsProps {
  planId: string;
}

export default function PlanJoinLeaveButtons({
  planId,
}: PlanJoinLeaveButtonsProps) {
  const { data: participated, isLoading } = useIsPlanParticipated(planId);
  const joinPlan = useJoinPlan(planId);
  const leavePlan = useLeavePlan(planId);
  const [confirmOpen, setConfirmOpen] = useState(false);

  if (isLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <CircularProgress />
      </Box>
    );
  }

  const handleConfirmLeave = () => {
    leavePlan.mutate();
    setConfirmOpen(false);
  };

  if (participated) {
    return (
      <>
        <Button
          variant="outlined"
          color="error"
          onClick={() => setConfirmOpen(true)}
          disabled={leavePlan.isPending}
          sx={{ width: "100%" }}
        >
          {leavePlan.isPending ? "Leaving..." : "Leave"}
        </Button>

        <Dialog open={confirmOpen} onClose={() => setConfirmOpen(false)}>
          <DialogTitle>Leave this plan?</DialogTitle>
          <DialogContent>
            <DialogContentText>
              Are you sure you want to leave this plan? if you are the owner you'll delete the plan</DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button variant="outlined" onClick={() => setConfirmOpen(false)}>Cancel</Button>
            <Button variant="outlined" color="error" onClick={handleConfirmLeave} autoFocus>
              Leave
            </Button>
          </DialogActions>
        </Dialog>
      </>
    );
  }

  return (
    <Button
      variant="contained"
      color="primary"
      onClick={() => joinPlan.mutate()}
      disabled={joinPlan.isPending}
      sx={{ width: "100%" }}
    >
      {joinPlan.isPending ? "Joining..." : "Join"}
    </Button>
  );
}
