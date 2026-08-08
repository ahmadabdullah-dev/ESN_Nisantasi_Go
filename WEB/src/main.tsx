import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { routes } from "./app/router/routes";
import { ThemeProvider, CssBaseline } from "@mui/material";
import theme from "./lib/theme";
import { RouterProvider } from "react-router";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <QueryClientProvider client={queryClient}>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
      <RouterProvider router={routes} />
    </LocalizationProvider>;
      </QueryClientProvider>
    </ThemeProvider>
  </StrictMode>,
);

