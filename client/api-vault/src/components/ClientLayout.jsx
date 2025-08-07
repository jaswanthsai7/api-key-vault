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

    // Unauthenticated trying to access protected
    if (!isAuthenticated && isProtectedRoute) {
      router.replace("/login");
      return;
    }

    // Authenticated user accessing public page
    if (isAuthenticated && isPublicRoute) {
      router.replace("/dashboard");
      return;
    }

    // Non-admin trying to access admin
    if (isAuthenticated && isAdminRoute && !isAdmin) {
      router.replace("/dashboard");
      return;
    }

    // Unknown route — client-safe 404 redirect
    if (
      isAuthenticated &&
      !isPublicRoute &&
      !isDashboardRoute &&
      !isAdminRoute
    ) {
      router.replace("/404"); // Make sure you have a /404 page
    }
  }, [loading, isAuthenticated, isAdmin, pathname]);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen text-sm text-gray-600">
        Checking authentication...
      </div>
    );
  }

  // Return nothing until redirection is handled
  if (
    (!isAuthenticated && isProtectedRoute) ||
    (isAuthenticated && isPublicRoute) ||
    (isAuthenticated && isAdminRoute && !isAdmin)
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
