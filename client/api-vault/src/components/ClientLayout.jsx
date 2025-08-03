"use client";

import { useEffect } from "react";
import { usePathname, useRouter } from "next/navigation";
import { useAuth } from "@/context/AuthContext";
import DashboardSidebar from "./DashboardSidebar";

export default function ClientLayout({ children }) {
  const { isAuthenticated, loading, isAdmin } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  const noSidebarRoutes = ["/login", "/register", "/"];
  const publicRoutes = ["/", "/login", "/register"];
  const showSidebar = !noSidebarRoutes.includes(pathname);
  const isPublic = publicRoutes.includes(pathname);
  useEffect(() => {
    if (loading) return; // Wait for auth loading to finish

    if (!isAuthenticated && !isPublic) {
      router.replace("/login");
    } else if (isAuthenticated && ["/login", "/register"].includes(pathname)) {
      router.replace("/dashboard");
    }
  }, [loading, isAuthenticated, pathname]);

  // Show loading screen only while checking token
  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen text-sm text-gray-600">
        Checking authentication...
      </div>
    );
  }
console.log(isAdmin);

  return (
    <div className="pt-14">
      {showSidebar && <DashboardSidebar/>}
      <div className={showSidebar ? "md:ml-64 p-4" : "p-4"}>{children}</div>
    </div>
  );
}
