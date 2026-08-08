import { Navigate, Outlet, useLocation } from "react-router";
import { useUser } from "../../lib/hooks/useUser";

export default function RequireAuth() {
  const { currentUser } = useUser();
  const location = useLocation();

  if (currentUser.isLoading) {
    return <div>Loading...</div>;
  }

  if (currentUser.isError || !currentUser.data) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <Outlet />;
}