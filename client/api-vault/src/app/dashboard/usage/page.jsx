"use client";

import { useEffect, useState } from "react";
import { format } from "date-fns";
import DashboardHeader from "@/components/DashboardHeader";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import { getUserAuditLogs } from "@/app/lib/usageService";
import PageLoader from "@/components/Loader"; 

function transformLogsToUsageStats(logs) {
  const usageMap = {};

  logs.forEach((log) => {
    const date = new Date(log.timestamp).toISOString().slice(0, 10); // "YYYY-MM-DD"
    usageMap[date] = (usageMap[date] || 0) + 1;
  });

  return Object.entries(usageMap)
    .map(([date, callCount]) => ({ date, callCount }))
    .sort((a, b) => new Date(a.date) - new Date(b.date));
}

export default function UsagePage() {
  const [logs, setLogs] = useState([]);
  const [usageStats, setUsageStats] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function loadData() {
      setLoading(true);
      try {
        const logData = await getUserAuditLogs();
        setLogs(logData);
        setUsageStats(transformLogsToUsageStats(logData));
      } catch (err) {
        console.error("Error fetching logs/stats:", err);
      }
      setLoading(false);
    }

    loadData();
  }, []);

  return (
    <>
      <DashboardHeader title="Usage & Logs" />

      {/* Chart Area */}
      <div className="bg-white p-6 rounded-lg border border-gray-200 shadow-sm mb-6">
        <p className="font-medium text-gray-700 mb-2">API Requests (Past 7 Days)</p>
        {loading ? (
          <div className="flex justify-center py-6">
            <PageLoader className="w-6 h-6 text-gray-500" />
          </div>
        ) : usageStats.length === 0 ? (
          <div className="text-gray-500 text-sm">No usage data available.</div>
        ) : (
          <ResponsiveContainer width="100%" height={200}>
            <BarChart data={usageStats}>
              <XAxis dataKey="date" />
              <YAxis allowDecimals={false} />
              <Tooltip />
              <Bar dataKey="callCount" fill="black" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        )}
      </div>

      {/* Logs Table */}
      <div className="bg-white p-6 rounded-lg border border-gray-200 shadow-sm">
        <p className="font-medium text-gray-700 mb-4">Recent API Logs</p>

        {loading ? (
          <div className="flex justify-center py-6">
            <PageLoader className="w-6 h-6 text-gray-500" />
          </div>
        ) : logs.length === 0 ? (
          <div className="text-gray-500 text-sm">No logs available.</div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full text-sm text-left text-gray-700">
              <thead>
                <tr>
                  <th className="pb-2 pr-4">Time</th>
                  <th className="pb-2 pr-4">Endpoint</th>
                  <th className="pb-2 pr-4">IP</th>
                  <th className="pb-2 pr-4">Status</th>
                </tr>
              </thead>
              <tbody>
                {logs.slice(0, 15).map((log, idx) => (
                  <tr key={idx} className="border-t border-gray-100">
                    <td className="py-2 pr-4">
                      {format(new Date(log.timestamp), "MMM dd, HH:mm")}
                    </td>
                    <td className="py-2 pr-4 font-mono text-xs truncate max-w-xs">
                      {log.endpoint}
                    </td>
                    <td className="py-2 pr-4 text-gray-500">{log.ipAddress}</td>
                    <td className="py-2 pr-4 font-semibold">
                      {log.statusCode === 200 ? (
                        <span className="text-green-600">{log.statusCode} OK</span>
                      ) : (
                        <span className="text-red-600">{log.statusCode}</span>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </>
  );
}
