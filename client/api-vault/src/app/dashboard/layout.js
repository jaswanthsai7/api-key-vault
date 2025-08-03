"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "@/context/AuthContext";
import DashboardSidebar from "@/components/DashboardSidebar";

export default function DashboardLayout({ children }) {


  return (
    <div className="flex min-h-screen pt-14">
      <main className="flex-1 px-8 py-10 bg-accent overflow-y-auto">
        {children}
      </main>
    </div>
  );
}
