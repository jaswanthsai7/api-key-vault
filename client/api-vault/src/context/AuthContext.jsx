"use client";

import api from "@/app/lib/api";
import { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);
  const [isAdmin, setIsAdmin] = useState(null);

  const checkAuth = async () => {
    try {
      const res = await api.verifyUser(); // secure /auth/me endpoint
      const role = res.data.role?.toLowerCase();
      setIsAdmin(role === "admin");
      setIsAuthenticated(true);
    } catch (err) {
      setIsAuthenticated(false);
      setIsAdmin(false);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    checkAuth();
  }, []);

  const login = async (isAdminFlag) => {
    setIsAuthenticated(true);
    setIsAdmin(isAdminFlag);
  };

  const logout = async () => {
    try {
      await api.post("/auth/logout");
    } catch (err) {
      console.error("Logout failed:", err);
    }
    setIsAuthenticated(false);
    setIsAdmin(false);
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
