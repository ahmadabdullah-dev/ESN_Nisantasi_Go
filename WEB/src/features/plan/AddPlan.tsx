import { useForm, Controller } from "react-hook-form";
import dayjs from "dayjs";
import type { AddPlanDto } from "../../lib/types/plan";
import { usePlan } from "../../lib/hooks/usePlan";
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

export default function AddPlan() {
  const { addPlanAsync } = usePlan();

  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<AddPlanDto>();

  const onSubmit = (creds: AddPlanDto) => {
    addPlanAsync.mutate(creds);
  };

  return (
    <Container maxWidth="sm">
      <Box sx={{ py: 6 }}>
        <Paper sx={{ p: 4 }}>
          <Typography variant="h4" sx={{ mb: 3 }}>
            Add a Plan
          </Typography>

          <Box component="form" onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={2}>
              <TextField
                label="Title"
                fullWidth
                {...register("title", { required: "Title is required" })}
                error={!!errors.title}
                helperText={errors.title?.message}
                disabled={addPlanAsync.isPending}
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
                disabled={addPlanAsync.isPending}
              />
              <TextField
                label="Location"
                fullWidth
                {...register("locationName")}
                disabled={addPlanAsync.isPending}
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
                        disabled: addPlanAsync.isPending,
                      },
                    }}
                  />
                )}
              />

              <Button
                type="submit"
                variant="contained"
                fullWidth
                disabled={addPlanAsync.isPending}
              >
                {addPlanAsync.isPending ? (
                  <CircularProgress size={24} color="inherit" />
                ) : (
                  "Add"
                )}
              </Button>
              {addPlanAsync.error && (
                <Alert severity="error">{addPlanAsync.error.message}</Alert>
              )}
              {addPlanAsync.isSuccess && (
                <Alert severity="success">{addPlanAsync.data}</Alert>
              )}
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
