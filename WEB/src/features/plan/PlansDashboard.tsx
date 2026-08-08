import { Button, Box, Divider } from "@mui/material";
import { useNavigate, Outlet } from "react-router";
import Plans from "./Plans";

export default function PlansDashboard() {
  const navigate = useNavigate();
  return (
    <Box sx={{ p: { xs: 2, sm: 3 } }}>
      <Box
        sx={{
          display: "flex",
          flexWrap: "wrap",
          gap: 1.5,
        }}
      >
        <Button
          variant="outlined"
          onClick={() => navigate("add")}
          sx={{ flex: { xs: "1 1 100%", sm: "0 1 200px" } }}
        >
          Add a plan
        </Button>
        <Divider sx={{ width: "100%", my: 1 }} />
        <Plans />
      </Box>
      <Outlet />
    </Box>
  );
}
