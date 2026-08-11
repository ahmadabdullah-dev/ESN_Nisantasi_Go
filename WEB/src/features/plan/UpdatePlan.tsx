import { useEffect } from "react";
import { useParams } from "react-router";
import { useForm, Controller } from "react-hook-form";
import dayjs from "dayjs";
import type { UpdatePlanDto } from "../../lib/types/plan";
import { usePlanById, useUpdatePlan } from "../../lib/hooks/usePlan";
import {
  Container,
  Paper,
  Typography,
  Box,
  Stack,
  TextField,
  Button,
  CircularProgress,
  Alert,
} from "@mui/material";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";

export default function UpdatePlan() {
  const { id } = useParams<{ id: string }>();

  const { data: plan, isLoading: isPlanLoading } = usePlanById(id!);
  const updatePlanAsync = useUpdatePlan();

  const {
    register,
    handleSubmit,
    control,
    reset,
    formState: { errors },
  } = useForm<UpdatePlanDto>();

  useEffect(() => {
    if (plan) {
      reset({
        planId: plan.id,
        title: plan.title,
        description: plan.description,
        locationName: plan.locationName,
        plannedAt: plan.plannedAt,
      });
    }
  }, [plan, reset]);

  const onSubmit = (creds: UpdatePlanDto) => {
    updatePlanAsync.mutate(creds);
  };

  if (isPlanLoading) {
    return (
      <Container maxWidth="sm">
        <Box sx={{ py: 6, display: "flex", justifyContent: "center" }}>
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  return (
    <Container maxWidth="sm">
      <Box sx={{ py: 6 }}>
        <Paper sx={{ p: 4 }}>
          <Typography variant="h4" sx={{ mb: 3 }}>
            Update Plan
          </Typography>

          <Box component="form" onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={2}>
              <TextField
                label="Title"
                fullWidth
                {...register("title", { required: "Title is required" })}
                error={!!errors.title}
                helperText={errors.title?.message}
                disabled={updatePlanAsync.isPending}
              />
              <TextField
                label="Description"
                fullWidth
                {...register("description", {
                  required: "Description is required",
                })}
                multiline
                minRows={5}
                error={!!errors.description}
                helperText={errors.description?.message}
                disabled={updatePlanAsync.isPending}
              />
              <TextField
                label="Location"
                fullWidth
                {...register("locationName")}
                disabled={updatePlanAsync.isPending}
              />

              <Controller
                name="plannedAt"
                control={control}
                rules={{ required: "Date and time are required" }}
                render={({ field, fieldState }) => (
                  <DateTimePicker
                    label="Planned Date & Time"
                    value={field.value ? dayjs(field.value as string) : null}
                    onChange={(newValue) =>
                      field.onChange(newValue ? newValue.toISOString() : "")
                    }
                    minDateTime={dayjs()}
                    slotProps={{
                      textField: {
                        fullWidth: true,
                        error: !!fieldState.error,
                        helperText: fieldState.error?.message,
                        disabled: updatePlanAsync.isPending,
                      },
                    }}
                  />
                )}
              />

              <Button
                type="submit"
                variant="contained"
                fullWidth
                disabled={updatePlanAsync.isPending}
              >
                {updatePlanAsync.isPending ? (
                  <CircularProgress size={24} color="inherit" />
                ) : (
                  "Save Changes"
                )}
              </Button>
              {updatePlanAsync.error && (
                <Alert severity="error">{updatePlanAsync.error.message}</Alert>
              )}
              {updatePlanAsync.isSuccess && (
                <Alert severity="success">{updatePlanAsync.data}</Alert>
              )}
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
