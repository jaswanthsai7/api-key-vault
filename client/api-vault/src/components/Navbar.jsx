"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useAuth } from "@/context/AuthContext";

export default function Navbar() {
  const { isAuthenticated, isAdmin, logout } = useAuth();
  const pathname = usePathname(); 

  return (
    <nav className="fixed top-0 left-0 right-0 h-14 flex items-center justify-between px-6 bg-white shadow z-50 border-b">
      <Link href="/" className="flex items-center gap-2">
        <img
          src="/window.svg"
          alt="API Vault Logo"
          width={28}
          height={28}
        />
        <span className="text-lg font-semibold text-gray-800 hidden sm:inline">API Vault</span>
      </Link>

      <div className="space-x-4">
        {isAuthenticated ? (
          <>
            <Link
              href={isAdmin ? "/admin" : "/dashboard"}
              className="text-sm font-medium text-gray-700 hover:underline"
            >
              {isAdmin ? "Admin Panel" : "Dashboard"}
            </Link>
            <button
              onClick={logout}
              className="text-sm text-red-500 hover:underline"
            >
              Logout
            </button>
          </>
        ) : (
          <>
            {pathname !== "/login" && (
              <Link href="/login" className="text-sm hover:underline">
                Login
              </Link>
            )}
            {pathname !== "/register" && (
              <Link href="/register" className="text-sm hover:underline">
                Register
              </Link>
            )}
          </>
        )}
      </div>
    </nav>
  );
}
