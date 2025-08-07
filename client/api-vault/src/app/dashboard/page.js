// src/app/dashboard/page.jsx or page.tsx

"use client";

import { useEffect, useState } from "react";
import DashboardHeader from "@/components/DashboardHeader";
import StatCard from "@/components/StatCard";
import { getDashboardStats } from "../lib/dashboardStatsService";
import PageLoader from "@/components/Loader";

export default function DashboardHome() {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchStats() {
      try {
        const data = await getDashboardStats();
        console.log(data);
        
        setStats(data);
      } catch (err) {
        console.error("Failed to load stats", err);
      } finally {
        setLoading(false);
      }
    }

    fetchStats();
  }, []);

  return (
    <>
      <DashboardHeader title="Dashboard Overview" />

      {loading ? (
        <div className="flex justify-center py-10">
          <PageLoader className="animate-spin w-6 h-6 text-gray-600" />
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-6 mt-4">
          <StatCard title="Total API Keys" value={stats.activeApiKeys + stats.expiredApiKeys} icon="key" />
          <StatCard title="Active Keys" value={stats.activeApiKeys} icon="check-circle" />
          <StatCard title="Expired Keys" value={stats.expiredApiKeys} icon="x-circle" />
          <StatCard title="API Calls (30d)" value={stats.apiCallsLast30Days} icon="bar-chart" />
        </div>
      )}

      <div className="mt-10">
        <h3 className="text-lg font-semibold mb-2 text-gray-800">Recent API Usage</h3>

        {stats?.recentUsage?.length > 0 ? (
          <div className="bg-white rounded-xl shadow overflow-x-auto border">
            <table className="min-w-full text-sm text-left">
              <thead className="bg-gray-50 text-gray-600">
                <tr>
                  <th className="px-6 py-3">Date</th>
                  <th className="px-6 py-3">Endpoint</th>
                  <th className="px-6 py-3">Calls</th>
                </tr>
              </thead>
              <tbody>
                {stats.recentUsage.map((entry, idx) => (
                  <tr
                    key={idx}
                    className="border-t hover:bg-gray-50 transition-colors"
                  >
                    <td className="px-6 py-4">{entry.date}</td>
                    <td className="px-6 py-4">{entry.endpoint}</td>
                    <td className="px-6 py-4 font-medium">{entry.calls}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : (
          <p className="text-sm text-gray-500">No usage data found.</p>
        )}
      </div>
    </>
  );
}
