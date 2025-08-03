"use client";

import { BarChart, CheckCircle, Key, XCircle } from "lucide-react";

const iconMap = {
  "key": Key,
  "check-circle": CheckCircle,
  "x-circle": XCircle,
  "bar-chart": BarChart,
};

export default function StatCard({ title, value, icon }) {
  const Icon = iconMap[icon] || Key;

  return (
    <div className="bg-white border border-gray-200 rounded-xl p-6 shadow-sm hover:shadow-md transition">
      <div className="flex items-center gap-4">
        <div className="p-3 rounded-full bg-gradient-to-br from-gray-100 to-gray-200 text-gray-700">
          <Icon className="w-5 h-5" />
        </div>
        <div>
          <p className="text-sm text-gray-500">{title}</p>
          <p className="text-xl font-bold text-gray-900">{value}</p>
        </div>
      </div>
    </div>
  );
}
