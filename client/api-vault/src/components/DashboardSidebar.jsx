"use client";

import { usePathname } from "next/navigation";
import Link from "next/link";
import { useState } from "react";
import { Menu, X } from "lucide-react";
import { useAuth } from "@/context/AuthContext";
import { adminRoutes, dashboardRoutes } from "@/app/constants/routesData";

export default function DashboardSidebar() {
  const pathname = usePathname();
  const [open, setOpen] = useState(false);
  const { isAdmin } = useAuth();

  const navItems = isAdmin ? adminRoutes : dashboardRoutes;

  return (
    <>
      {/* Mobile Toggle Button */}
      <button
        className="md:hidden fixed top-[0.75rem] left-4 z-50 bg-black text-white p-2 rounded"
        onClick={() => setOpen(!open)}
      >
        {open ? <X size={20} /> : <Menu size={20} />}
      </button>

      {/* Mobile Overlay */}
      {open && (
        <div
          onClick={() => setOpen(false)}
          className="fixed inset-0 bg-black/50 z-40 md:hidden"
        />
      )}

      {/* Sidebar */}
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
              prefetch={true}
              onClick={() => setOpen(false)}
              className={`block px-4 py-2 rounded-md text-sm font-medium ${
                pathname.startsWith(href)
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
