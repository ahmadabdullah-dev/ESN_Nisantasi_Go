import {
  Avatar,
  Box,
  Chip,
  CircularProgress,
  Divider,
  Stack,
  Typography,
} from "@mui/material";
import { useGetUserByUsername } from "../../lib/hooks/useUser";
import UserPlans from "../plan/UserPlans";
import { useParams } from "react-router";

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

export interface ProfileProps {
  userName?: string;
}

export default function Profile({ userName: userNameProp }: ProfileProps) {
  const { username: userNameParam } = useParams<{ username: string }>();
  const userName = userNameProp ?? userNameParam;

  const user = useGetUserByUsername(userName ?? "");

  if (!userName) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <Typography color="text.secondary">No username provided.</Typography>
      </Box>
    );
  }

  if (user.isLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (user.isError) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <Typography color="error">
          Failed to load profile. Please try again.
        </Typography>
      </Box>
    );
  }

  const data = user.data;

  if (!data) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <Typography color="text.secondary">No profile data found.</Typography>
      </Box>
    );
  }

  const fullName = [data.firstName, data.lastName].filter(Boolean).join(" ");

  return (
    <Box
      sx={{
        width: "100%",
        minHeight: "100%",
        p: { xs: 2, sm: 4 },
      }}
    >
      <Stack
        spacing={1.5}
        sx={{ alignItems: "center", textAlign: "center", mb: 4 }}
      >
        <Avatar
          src={data.profilePhotoPublicId || undefined}
          sx={{ width: 88, height: 88, fontSize: 32 }}
        >
          {fullName[0] || data.userName?.[0]}
        </Avatar>

        <Box>
          <Typography variant="h6" sx={{ fontWeight: 600 }}>
            {fullName || data.userName}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            @{data.userName}
          </Typography>
        </Box>

        <Chip
          label={data.isActive ? "Active" : "Inactive"}
          color={data.isActive ? "success" : "default"}
          size="small"
          variant={data.isActive ? "filled" : "outlined"}
        />
      </Stack>

      <Divider sx={{ mb: 4 }} />

      <Stack
        direction={{ xs: "column", sm: "row" }}
        spacing={{ xs: 2.5, sm: 4 }}
        sx={{ mb: 4 }}
      >
        <Box sx={{ flex: 1 }}>
          <ProfileField label="Email" value={data.email} />
        </Box>
        <Box sx={{ flex: 1 }}>
          <ProfileField label="Country" value={data.country} />
        </Box>
        <Box sx={{ flex: 1 }}>
          <ProfileField label="Department" value={data.department} />
        </Box>
      </Stack>

      <UserPlans userId={data.id} />
    </Box>
  );
}
