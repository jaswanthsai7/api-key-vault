"use client";

import { useEffect, useState } from "react";
import MultiShimmer from "@/components/MultiShimmer";

export default function AdminHome() {
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Simulate loading delay (replace this with actual data fetching if needed)
    const timer = setTimeout(() => setLoading(false), 1000);
    return () => clearTimeout(timer);
  }, []);

  if (loading) {
    return (
      <div className="mt-6">
        <MultiShimmer
          count={1}
          type="card"
          columns={1}
          height="h-4"
          width="w-full"
        />
      </div>
    );
  }

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 mb-4">Admin Dashboard</h1>
      <p className="text-gray-700 text-sm">
        Manage users, API usage, and system-wide settings here.
      </p>
    </div>
  );
}
