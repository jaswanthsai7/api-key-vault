"use client";
import { useEffect } from "react";
import { usePathname, useRouter, notFound } from "next/navigation";
import { useAuth } from "@/context/AuthContext";
import DashboardSidebar from "./DashboardSidebar";

export default function ClientLayout({ children }) {
  const { isAuthenticated, loading, isAdmin } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  const publicRoutes = ["/", "/login", "/register"];
  const adminPrefix = "/admin";
  const dashboardPrefix = "/dashboard";

  const isPublic = publicRoutes.includes(pathname);
  const showSidebar = !publicRoutes.includes(pathname);

  useEffect(() => {
    if (loading) return;

    if (!isAuthenticated && !isPublic) {
      router.replace("/login");
      return;
    }

    if (isAuthenticated && isPublic) {
      router.replace("/dashboard");
      return;
    }

    if (isAuthenticated && pathname.startsWith(adminPrefix) && !isAdmin) {
      router.replace("/dashboard");
      return;
    }

    const isKnownPath =
      isPublic ||
      pathname.startsWith(dashboardPrefix) ||
      pathname.startsWith(adminPrefix);

    if (isAuthenticated && !isKnownPath) {
      notFound();
    }
  }, [loading, isAuthenticated, pathname, isAdmin]);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen text-sm text-gray-600">
        Checking authentication...
      </div>
    );
  }

  return (
    <div className="pt-14">
      {showSidebar && <DashboardSidebar />}
      <div className={showSidebar ? "md:ml-64 p-4" : "p-4"}>{children}</div>
    </div>
  );
}