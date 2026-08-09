import { Box, Typography } from "@mui/material";
import LogoutButton from "../auth/LogoutButton";

export default function SettingsHeader() {
  return (
    <> 
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
    
      }}
    >
      <Typography variant="h5" sx={{ fontWeight: 600 }}>
        Settings
      </Typography>
      <LogoutButton />
    </Box>
    </>
   

  );
}
