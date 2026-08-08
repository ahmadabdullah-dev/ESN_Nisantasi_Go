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
} from "@mui/material"
import { useUser } from "../../lib/hooks/useUser"

interface ProfileFieldProps {
  label: string
  value?: string | number | null
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
  )
}

export default function Profile() {
  const { currentUser } = useUser()
  const user = currentUser.data

  if (currentUser.isLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
        <CircularProgress />
      </Box>
    )
  }

  if (!user) {
    return (
      <Box sx={{ mt: 6, textAlign: "center" }}>
        <Typography color="text.secondary">No profile data found.</Typography>
      </Box>
    )
  }

  const fullName = [user.firstName, user.lastName].filter(Boolean).join(" ")

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
          <Stack spacing={1.5} sx={{ alignItems: "center", textAlign: "center" }}>
            <Avatar
              src={
                user.profilePhotoPublicId
                  ? `https://res.cloudinary.com/CLOUD_NAME/image/upload/${user.profilePhotoPublicId}`
                  : undefined
              }
              sx={{ width: 88, height: 88, fontSize: 32 }}
            >
              {fullName?.[0] || user.userName?.[0]}
            </Avatar>

            <Box>
              <Typography variant="h6" sx={{ fontWeight: 600 }}>
                {fullName || user.userName}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                @{user.userName}
              </Typography>
            </Box>

            <Chip
              label={user.isActive ? "Active" : "Inactive"}
              color={user.isActive ? "success" : "default"}
              size="small"
              variant={user.isActive ? "filled" : "outlined"}
            />
          </Stack>

          <Divider sx={{ my: 3 }} />

          <Stack spacing={2.5}>
            <ProfileField label="Email" value={user.email} />
            <Stack direction="row" spacing={4}>
              <Box sx={{ flex: 1 }}>
                <ProfileField label="Country" value={user.country} />
              </Box>
              <Box sx={{ flex: 1 }}>
                <ProfileField label="Department" value={user.department} />
              </Box>
            </Stack>
          </Stack>
        </CardContent>
      </Card>
    </Box>
  )
}