"use client";

import { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true); //  Prevent flickering
  const [isAdmin, setIsAdmin] = useState(null);
  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    const storedIsAdmin =  localStorage.getItem("isAdminRole");
    if (storedToken) {
      setToken(storedToken);
      setToken(storedIsAdmin);
      setIsAuthenticated(true);
    }
    setLoading(false); //  Auth check complete
  }, []);

  const login = (token,isAdmin) => {
    localStorage.setItem("token", token);
    localStorage.setItem("isAdminRole", isAdmin);
    console.log(isAdmin);
    
    setToken(token);
    setIsAdmin(isAdmin)
    setIsAuthenticated(true);
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("isAdminRole");
    setToken(null);
    setIsAdmin(false)
    setIsAuthenticated(false);
  };


  return (
    <AuthContext.Provider
      value={{ token, isAuthenticated, login, logout, loading ,isAdmin}}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
