import {
  Avatar,
  Box,
  Card,
  CardContent,
  Chip,
  CircularProgress,
  Divider,
  Stack,
  Typography,
} from "@mui/material";
import { useCurrentUser } from "../../lib/hooks/useUser";

interface ProfileFieldProps {
  label: string;
  value?: string | number | null;
}

function ProfileField({ label, value }: ProfileFieldProps) {
  return (
    <Box>
      <Typography variant="caption" color="text.secondary">
        {label}
      </Typography>
      <Typography variant="body1" sx={{ fontWeight: 500 }}>
        {value || "—"}
      </Typography>
    </Box>
  );
}

export default function MyProfile() {
  const currentUser = useCurrentUser();

  if (currentUser.isLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (!currentUser.data) {
    return (
      <Box sx={{ mt: 6, textAlign: "center" }}>
        <Typography color="text.secondary">No profile data found.</Typography>
      </Box>
    );
  }

  const fullName = [currentUser.data.firstName, currentUser.data.lastName].filter(Boolean).join(" ");

  return (
    <Box sx={{ display: "flex", justifyContent: "center", mt: 4, px: 2 }}>
      <Card
        elevation={0}
        sx={{
          maxWidth: 480,
          width: "100%",
          borderRadius: 3,
          border: "1px solid",
          borderColor: "divider",
        }}
      >
        <CardContent sx={{ p: 4 }}>
          <Stack
            spacing={1.5}
            sx={{ alignItems: "center", textAlign: "center" }}
          >
            <Avatar
              src={
                currentUser.data.profilePhotoPublicId
                  ? `https://res.cloudinary.com/CLOUD_NAME/image/upload/${currentUser.data.profilePhotoPublicId}`
                  : undefined
              }
              sx={{ width: 88, height: 88, fontSize: 32 }}
            >
              {fullName?.[0] || currentUser.data.userName?.[0]}
            </Avatar>

            <Box>
              <Typography variant="h6" sx={{ fontWeight: 600 }}>
                {fullName || currentUser.data.userName}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                @{currentUser.data.userName}
              </Typography>
            </Box>

            <Chip
              label={currentUser.data.isActive ? "Active" : "Inactive"}
              color={currentUser.data.isActive ? "success" : "default"}
              size="small"
              variant={currentUser.data.isActive ? "filled" : "outlined"}
            />
          </Stack>

          <Divider sx={{ my: 3 }} />

          <Stack spacing={2.5}>
            <ProfileField label="Email" value={currentUser.data.email} />
            <Stack direction="row" spacing={4}>
              <Box sx={{ flex: 1 }}>
                <ProfileField label="Country" value={currentUser.data.country} />
              </Box>
              <Box sx={{ flex: 1 }}>
                <ProfileField label="Department" value={currentUser.data.department} />
              </Box>
            </Stack>
          </Stack>
        </CardContent>
      </Card>
    </Box>
  );
}
