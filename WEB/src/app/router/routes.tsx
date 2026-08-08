import { createBrowserRouter, Navigate } from "react-router";
import App from "../components/App";
import ErrorPage from "../../features/errors/ErrorPage";
import NotFound from "../../features/errors/NotFound";
import LoginForm from "../../features/auth/LoginForm";
import RequireAuth from "./RequireAuth";
import AdminDashboard from "../../features/admin/AdminDashboard";
import Dashboard from "../components/Dashboard";
import Profile from "../../features/user/My-Profile";
import Plans from "../../features/plan/Plans";

export const routes = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    errorElement: <ErrorPage />,
    children: [
      { index: true, element: <Navigate to="/dashboard" replace /> },
       {
        element: <RequireAuth />,
        children: [
          { path: "admin", element: <AdminDashboard /> },
          { path: "dashboard", element: <Dashboard /> },
          { path: "my-profile", element: <Profile/>},
          { path: "plans", element: <Plans/>}

        ],
      },
      { path: "login", element: <LoginForm /> },
    ],
  },
  { path: "*", element: <NotFound /> },
]);