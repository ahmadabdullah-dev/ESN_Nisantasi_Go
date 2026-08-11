import { Container, CircularProgress, Box, Typography } from "@mui/material";
import Profile from "../user/Profile";
import SettingsHeader from "./SettingsHeader";
import { useCurrentUser } from "../../lib/hooks/useUser";

export default function SettingsDashboard() {
  const currentUser = useCurrentUser();

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <SettingsHeader />

      {currentUser.isLoading ? (
        <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
          <CircularProgress />
        </Box>
      ) : currentUser.data ? (
        <Profile userName={currentUser.data.userName} />
      ) 
      : (
        <Box sx={{ mt: 6, textAlign: "center" }}>
          <Typography color="text.secondary">
            Unable to load your profile.
          </Typography>
        </Box>
      )}
    </Container>
  );
}
