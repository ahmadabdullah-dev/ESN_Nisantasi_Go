import { useState } from "react";
import type { RegisterUserDto } from "../../../lib/types/admin";
import {
  Container,
  CircularProgress,
  Alert,
  Box,
  Paper,
  Typography,
  TextField,
  InputAdornment,
  IconButton,
  Button,
  Stack,
} from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { useForm } from "react-hook-form";
import { useRegisterAdmin } from "../../../lib/hooks/useAdmin";

export default function RegisterMember() {
    const registerAdmin = useRegisterAdmin();
    const {
    register,
    handleSubmit,
    reset,
    resetField,
    formState: { errors },
  } = useForm<RegisterUserDto>();
  const [showPassword, setShowPassword] = useState(false);

  const onSubmit = (creds: RegisterUserDto) => {
    registerAdmin.mutateAsync(creds, {
      onSuccess: () => {
        reset();
      },
      onError: () => {
        resetField("password");
      },
    });
  };
  return (
    <Container maxWidth="sm">
      <Box>
        <Paper sx={{ p: 4, width: "100%" }}>
          <Typography
            variant="h3"
            sx={{
              m: 2,
              textAlign: "center",
            }}
          >
            Register Admin
          </Typography>

          <Box component="form" onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={2}>
              <TextField
                label="FirstName"
                fullWidth
                {...register("firstName", {
                  required: "FirstName is required",
                })}
                error={!!errors.firstName}
                helperText={errors.firstName?.message}
              />
              <TextField
                label="LastName"
                fullWidth
                {...register("lastName", { required: "LastName is required" })}
                error={!!errors.lastName}
                helperText={errors.lastName?.message}
              />
              <TextField
                label="Email"
                type="email"
                fullWidth
                {...register("email", { required: "Email is required" })}
                error={!!errors.email}
                helperText={errors.email?.message}
              />
              <TextField
                label="Country"
                fullWidth
                {...register("country", { required: "Country is required" })}
                error={!!errors.country}
                helperText={errors.country?.message}
              />
              <TextField
                label="Department"
                fullWidth
                {...register("department", {
                  required: "Department is required",
                })}
                error={!!errors.department}
                helperText={errors.department?.message}
              />
              <TextField
                label="Password"
                type={showPassword ? "text" : "password"}
                {...register("password", {
                  required: "Password is required",
                  minLength: {
                    value: 8,
                    message: "Must be at least 8 characters",
                  },
                })}
                error={!!errors.password}
                helperText={errors.password?.message}
                fullWidth
                slotProps={{
                  input: {
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton
                          onClick={() => setShowPassword(!showPassword)}
                          edge="end"
                        >
                          {showPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                      </InputAdornment>
                    ),
                  },
                }}
              />
              <Button
                type="submit"
                variant="contained"
                fullWidth
                disabled={registerAdmin.isPending}
              >
                {registerAdmin.isPending ? (
                  <CircularProgress size={24} color="inherit" />
                ) : (
                  "Register Admin"
                )}
              </Button>
              {registerAdmin.isSuccess && (
                <Alert severity="success">{registerAdmin.data}</Alert>
              )}
              {registerAdmin.error && (
                <Alert severity="error">
                  {registerAdmin.error.message }
                </Alert>
              )}
            </Stack>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}
