"use client";

import { usePathname } from "next/navigation";
import Link from "next/link";
import { useState, useEffect } from "react";
import { Menu, X } from "lucide-react";

export default function DashboardSidebar({ isAdmin = false }) {
  const pathname = usePathname();
  const [open, setOpen] = useState(false);

  useEffect(() => {
    setOpen(false);
  }, [pathname]);

  const navItems = isAdmin
    ? [
        { href: "/admin", label: "Admin Home" },
        { href: "/admin/users", label: "Users" },
        { href: "/admin/audit", label: "Audit Logs" },
      ]
    : [
        { href: "/dashboard", label: "Overview" },
        { href: "/dashboard/keys", label: "API Keys" },
        { href: "/dashboard/usage", label: "Usage & Logs" },
        // { href: "/dashboard/account", label: "Account Settings" },
        { href: "/dashboard/testApi", label: "Verify API Key" },
      ];

  return (
    <>
      {/* Mobile toggle */}
      <button
        className="md:hidden fixed top-4 left-4 z-50 bg-black text-white p-2 rounded"
        onClick={() => setOpen(!open)}
      >
        {open ? <X size={20} /> : <Menu size={20} />}
      </button>

      {/* Mobile overlay */}
      {open && (
        <div
          onClick={() => setOpen(false)}
          className="fixed inset-0 bg-black/50 z-40 md:hidden"
        />
      )}

      {/* Fixed Sidebar */}
      <aside
        className={`fixed top-14 left-0 w-64 h-[calc(100vh-56px)] bg-white border-r z-40
          transform transition-transform duration-300 ease-in-out
          ${open ? "translate-x-0" : "-translate-x-full"}
          md:translate-x-0 md:block`}
      >
        <div className="p-4 space-y-2">
          {navItems.map(({ href, label }) => (
            <Link
              key={href}
              href={href}
              className={`block px-4 py-2 rounded-md text-sm font-medium ${
                pathname === href
                  ? "bg-black text-white"
                  : "text-gray-700 hover:bg-gray-100"
              }`}
            >
              {label}
            </Link>
          ))}
        </div>
      </aside>
    </>
  );
}
