import { useState } from "react";
import { useNavigate } from "react-router";
import type { PaginationParams } from "../../lib/types/common";
import {
  Alert,
  Box,
  Button,
  Card,
  CardContent,
  Chip,
  Container,
  Grid,
  Pagination,
  Skeleton,
  Stack,
  Typography,
} from "@mui/material";
import PersonOutlineIcon from "@mui/icons-material/Person4Outlined";
import PlaceOutlinedIcon from "@mui/icons-material/PlaceOutlined";
import CalendarMonthOutlinedIcon from "@mui/icons-material/CalendarMonthOutlined";
import { Shared } from "../../lib/shared";
import BoltIcon from "@mui/icons-material/Bolt";
import { useGetPlansAsync } from "../../lib/hooks/usePlan";

export default function Plans() {
  const [pagination, setPagination] = useState<PaginationParams>({
    page: 1,
    pageSize: 9,
  });
  const  getPlansAsync  = useGetPlansAsync(pagination);
  const navigate = useNavigate();

  if (getPlansAsync.isLoading) {
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

  if (getPlansAsync.isError || !getPlansAsync.data) {
    return (
      <Container maxWidth="lg" sx={{ py: 8 }}>
        <Alert severity="error" variant="outlined">
          Something went wrong while loading plans. Please try again.
        </Alert>
      </Container>
    );
  }

  const list = getPlansAsync.data;

  return (
    <Container maxWidth="lg" sx={{ py: 8 }}>
      <Typography variant="h4" sx={{ mb: 4, fontWeight: 700 }}>
        Plans
      </Typography>

      {list.items.length === 0 ? (
        <Alert severity="info" variant="outlined">
          No plans found.
        </Alert>
      ) : (
        <Grid container spacing={3}>
          {list.items.map((p) => (
            <Grid key={p.id ?? p.title} size={{ xs: 12, sm: 6, md: 4 }}>
              <Card
                variant="outlined"
                sx={{
                  display: "flex",
                  flexDirection: "column",
                  height: "100%",
                }}
              >
                <CardContent sx={{ flexGrow: 1 }}>
                  <Typography variant="subtitle1" sx={{ fontWeight: 700, mb: 0.5 }}>
                    {p.title}
                  </Typography>

                  {p.description && (
                    <Typography
                      variant="body2"
                      sx={{
                        color: "text.secondary",
                        mb: 2,
                      }}
                    >
                      {p.description}
                    </Typography>
                  )}

                  <Stack spacing={1} sx={{ mb: p.creatorUserName ? 2 : 0 }}>
                    {p.locationName && (
                      <Stack direction="row" spacing={1} sx={{alignItems:"center"}}>
                        <PlaceOutlinedIcon fontSize="small" sx={{ color: "text.secondary" }} />
                        <Typography
                          variant="body2"
                          sx={{
                            color: "text.secondary",
                            whiteSpace: "nowrap",
                          }}
                        >
                          {p.locationName}
                        </Typography>
                      </Stack>
                    )}

                    <Stack direction="row" sx= {{alignItems:"center"}} spacing={1}>
                      <CalendarMonthOutlinedIcon fontSize="small" sx={{ color: "text.secondary" }} />
                      <Typography variant="body2" sx={{ color: "text.secondary" }}>
                        {Shared.formatDate(p.plannedAt) ?? "Date not set"}
                      </Typography>
                    </Stack>
                  </Stack>

                  {p.creatorUserName && (
                    <Chip
                      size="small"
                      variant="outlined"
                      icon={<PersonOutlineIcon sx={{ fontSize: 16 }} />}
                      label={p.creatorUserName}
                      sx={{ maxWidth: "100%" }}
                    />
                  )}
                </CardContent>

                <Box sx={{ p: 2, pt: 0 }}>
                  <Button
                    fullWidth
                    variant= "outlined"
                    endIcon={<BoltIcon />}
                    onClick={() => navigate(`/plans/${p.id}`)}
                  >
                  Details...
                  </Button>
                </Box>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}

      {list.totalPages > 1 && (
        <Box sx={{ display: "flex", justifyContent: "center", mt: 6 }}>
          <Pagination
            count={list.totalPages}
            page={list.currentPage}
            onChange={(_, page) => setPagination((p) => ({ ...p, page }))}
          />
        </Box>
      )}
    </Container>
  );
}