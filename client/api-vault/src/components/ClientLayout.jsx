"use client";

import { useEffect } from "react";
import { useRouter, usePathname } from "next/navigation";
import { useAuth } from "@/context/AuthContext";
import DashboardSidebar from "./DashboardSidebar";

export default function ClientLayout({ children }) {
  const { isAuthenticated, loading, isAdmin } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  const publicRoutes = ["/", "/login", "/register"];
  const adminPrefix = "/admin";
  const dashboardPrefix = "/dashboard";

  const isPublicRoute = publicRoutes.includes(pathname);
  const isAdminRoute = pathname.startsWith(adminPrefix);
  const isDashboardRoute = pathname.startsWith(dashboardPrefix);
  const isProtectedRoute = isAdminRoute || isDashboardRoute;
  const showSidebar = isProtectedRoute;

  useEffect(() => {
    if (loading) return;

    // Not authenticated & accessing protected route
    if (!isAuthenticated && isProtectedRoute) {
      router.replace("/login");
      return;
    }

    // Authenticated & trying to access public route (redirect to correct dashboard)
    if (isAuthenticated && isPublicRoute) {
      router.replace(isAdmin ? "/admin" : "/dashboard");
      return;
    }

    // Non-admin trying to access /admin
    if (isAuthenticated && isAdminRoute && !isAdmin) {
      router.replace("/dashboard");
      return;
    }

    // Admin trying to access /dashboard
    if (isAuthenticated && isDashboardRoute && isAdmin) {
      router.replace("/admin");
      return;
    }

    // Invalid/unknown route
    if (
      isAuthenticated &&
      !isPublicRoute &&
      !isAdminRoute &&
      !isDashboardRoute
    ) {
      router.replace("/404"); // Make sure this route exists
    }
  }, [loading, isAuthenticated, isAdmin, pathname]);

  // Show loader until check completes
  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen text-sm text-gray-600">
        Checking authentication...
      </div>
    );
  }

  // Prevent rendering layout if user is being redirected
  if (
    (!isAuthenticated && isProtectedRoute) ||
    (isAuthenticated && isPublicRoute) ||
    (isAuthenticated && isAdminRoute && !isAdmin) ||
    (isAuthenticated && isDashboardRoute && isAdmin)
  ) {
    return null;
  }

  return (
    <div className="pt-14">
      {showSidebar && <DashboardSidebar />}
      <div className={showSidebar ? "md:ml-64 p-4" : "p-4"}>{children}</div>
    </div>
  );
}
