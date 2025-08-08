"use client";

import { logoutUser, verifyUser } from "@/app/lib/authService";
import { createContext, useContext, useEffect, useState } from "react";
import { usePathname } from "next/navigation";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);
  const [isAdmin, setIsAdmin] = useState(false); // default to false
  const pathname = usePathname();
  const checkAuth = async () => {
    try {
      const res = await verifyUser(); // e.g., /auth/me
      const role = res?.role?.toLowerCase();
      setIsAuthenticated(true);
      setIsAdmin(role.toLowerCase() === "admin");
    } catch (err) {
      setIsAuthenticated(false);
      setIsAdmin(false);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    // Only check auth for protected routes
    const publicRoutes = ["/", "/login", "/register"];
    if (!publicRoutes.includes(pathname)) {
      checkAuth();
    } else {
      setLoading(false); // skip check
    }
  }, [pathname]);


  const login = async (isAdminFlag = false) => {
    setIsAuthenticated(true);
    setIsAdmin(isAdminFlag);
    window.location.replace(isAdminFlag ? "/admin" : "/dashboard");
  };

  const logout = async () => {
    try {
      await logoutUser(); // API invalidates session or token
    } catch (err) {
      console.error("Logout failed:", err);
    } finally {
      setIsAuthenticated(false);
      setIsAdmin(false);
    }
  };

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, isAdmin, login, logout, loading }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
