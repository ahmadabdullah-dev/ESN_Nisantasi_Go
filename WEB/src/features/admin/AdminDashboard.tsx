import { Box, CircularProgress, Typography } from "@mui/material";
import { useCurrentUser } from "../../lib/hooks/useUser";
import RegisterAdmin from "./Register/RegisterADmin";
import RegisterMember from "./Register/RegisterMember";

export default function AdminDashboard() {
  const currentUser = useCurrentUser();

  if (currentUser.isLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <CircularProgress />
      </Box>
    );
  }
  const role = currentUser.data?.role;

  return (
    <Box sx={{ p: 4 }}>
      <Typography variant="h5" sx={{ fontWeight: 600, mb: 3 }}>
        Admin Dashboard
      </Typography>
      <RegisterMember />
      {role === "SuperAdmin" && (
        <>
          <RegisterAdmin />
        </>
      )}
    </Box>
  );
}
