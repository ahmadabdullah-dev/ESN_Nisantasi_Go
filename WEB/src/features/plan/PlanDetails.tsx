import { useParams } from "react-router";
import { usePlanById } from "../../lib/hooks/usePlan";
import {
  Alert,
  Box,
  Card,
  CardContent,
  Chip,
  Container,
  Grid,
  Skeleton,
  Stack,
  Typography,
} from "@mui/material";
import PersonOutlineIcon from "@mui/icons-material/Person4Outlined";
import PlaceOutlinedIcon from "@mui/icons-material/PlaceOutlined";
import CalendarMonthOutlinedIcon from "@mui/icons-material/CalendarMonthOutlined";
import { Shared } from "../../lib/shared";

export default function PlanDetails() {
  const { id } = useParams<{ id: string }>();
  const { data: plan, isLoading, error } = usePlanById(id ?? "");

  if (isLoading) {
    return (
      <Container maxWidth="lg" sx={{ py: 8 }}>
        <Skeleton variant="text" width={160} height={56} sx={{ mb: 4 }} />
        <Grid container spacing={3}>
          {Array.from({ length: 6 }).map((_, i) => (
            <Grid key={i} size={{ xs: 12, sm: 6, md: 4 }}>
              <Skeleton variant="rectangular" height={220} />
            </Grid>
          ))}
        </Grid>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 8 }}>
        <Alert severity="error" variant="outlined">
          Something went wrong while loading the plan. Please try again.
        </Alert>
      </Container>
    );
  }

  if (!plan) {
    return (
      <Box sx={{ maxWidth: 600, mx: "auto", mt: 4 }}>
        <Alert severity="info">Plan not found.</Alert>
      </Box>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 8 }}>
      <Card>
        <CardContent sx={{ flexGrow: 1 }}>
          <Typography variant="subtitle1" sx={{ fontWeight: 700, mb: 0.5 }}>
            {plan.title}
          </Typography>

          {plan.description && (
            <Typography
              variant="body2"
              sx={{
                color: "text.secondary",
                mb: 2,
              }}
            >
              {plan.description}
            </Typography>
          )}

          <Stack spacing={1} sx={{ mb: plan.creatorUserName ? 2 : 0 }}>
            {plan.locationName && (
              <Stack direction="row" spacing={1} sx={{ alignItems: "center" }}>
                <PlaceOutlinedIcon
                  fontSize="small"
                  sx={{ color: "text.secondary" }}
                />
                <Typography
                  variant="body2"
                  sx={{
                    color: "text.secondary",
                    whiteSpace: "nowrap",
                  }}
                >
                  {plan.locationName}
                </Typography>
              </Stack>
            )}

            <Stack direction="row" sx={{ alignItems: "center" }} spacing={1}>
              <CalendarMonthOutlinedIcon
                fontSize="small"
                sx={{ color: "text.secondary" }}
              />
              <Typography variant="body2" sx={{ color: "text.secondary" }}>
                {Shared.formatDate(plan.plannedAt) ?? "Date not set"}
              </Typography>
            </Stack>
          </Stack>

          {plan.creatorUserName && (
            <Chip
              size="small"
              variant="outlined"
              icon={<PersonOutlineIcon sx={{ fontSize: 16 }} />}
              label={plan.creatorUserName}
              sx={{ maxWidth: "100%" }}
            />
          )}
        </CardContent>
      </Card>
    </Container>
  );
}
