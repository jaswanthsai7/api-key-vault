"use client";

import { useAuth } from "@/context/AuthContext";
import Link from "next/link";


export default function Navbar() {
  const { isAuthenticated, logout } = useAuth();

  return (
    <header className="fixed top-0 left-0 w-full h-14 z-50 bg-white/80 border-b backdrop-blur-md flex items-center px-4">
      <Link href="/" className="text-lg font-semibold text-gray-800">
        API Vault
      </Link>

      <div className="ml-auto flex items-center gap-4">
        {!isAuthenticated ? (
          <>
            <Link href="/login" className="text-sm text-gray-600 hover:text-gray-900">
              Login
            </Link>
            <Link
              href="/register"
              className="bg-accent hover:bg-accentHover px-4 py-1.5 rounded-md text-sm font-medium text-gray-900 border"
            >
              Get Started
            </Link>
          </>
        ) : (
          <>
            <Link href="/dashboard" className="text-sm text-gray-600 hover:text-gray-900">
              Dashboard
            </Link>
            <button
              onClick={logout}
              className="text-sm text-red-600 hover:text-red-800"
            >
              Logout
            </button>
          </>
        )}
      </div>
    </header>
  );
}
