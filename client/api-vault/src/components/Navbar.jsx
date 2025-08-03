"use client";

import Link from "next/link";

export default function Navbar() {
  return (
    <header className="fixed top-0 left-0 w-full h-14 z-50 bg-white/80 border-b backdrop-blur-md flex items-center px-4">
      <Link href="/" className="text-lg font-semibold text-gray-800">
        API Vault
      </Link>

      <div className="ml-auto flex items-center gap-4">
        <Link href="/login" className="text-sm text-gray-600 hover:text-gray-900">
          Login
        </Link>
        <Link
          href="/register"
          className="bg-accent hover:bg-accentHover px-4 py-1.5 rounded-md text-sm font-medium text-gray-900 border"
        >
          Get Started
        </Link>
      </div>
    </header>
  );
}
