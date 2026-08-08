import { createBrowserRouter, Navigate } from "react-router";
import App from "../components/App";
import ErrorPage from "../../features/errors/ErrorPage";
import NotFound from "../../features/errors/NotFound";
import LoginForm from "../../features/auth/LoginForm";
import RequireAuth from "./RequireAuth";
import AdminDashboard from "../../features/admin/AdminDashboard";
import Dashboard from "../components/Dashboard";
import Profile from "../../features/user/Profile";
import MyProfile from "../../features/user/MyProfile";
import PlansDashboard from "../../features/plan/PlansDashboard";
import AddPlan from "../../features/plan/AddPlan";

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
          { path: "my-profile", element: <MyProfile /> },
          { path: "profile/:username", element: <Profile /> },
          { path: "plans", element: <PlansDashboard /> },
          { path: "plans/add", element: <AddPlan /> },
        ],
      },
      { path: "login", element: <LoginForm /> },
    ],
  },
  { path: "*", element: <NotFound /> },
]);