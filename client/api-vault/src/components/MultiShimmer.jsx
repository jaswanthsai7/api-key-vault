"use client";

import React from "react";

export default function MultiShimmer({
  count = 4,
  type = "card", // card | list | line
  columns = 1,
  width = "w-full",
  height = "h-48",
  className = "",
}) {
  const renderShimmer = (key) => {
    switch (type) {
      case "list":
        return (
          <div
            key={key}
            className="animate-pulse flex justify-between items-center px-6 py-4 bg-white border-b"
          >
            <div className="h-4 bg-gray-200 rounded w-1/4" />
            <div className="h-4 bg-gray-200 rounded w-1/2" />
            <div className="h-4 bg-gray-200 rounded w-1/6" />
          </div>
        );

      case "line":
        return (
          <div
            key={key}
            className={`animate-pulse bg-gray-200 rounded my-2 ${height} ${width}`}
          />
        );

      case "card":
      default:
        return (
          <div
            key={key}
            className={`animate-pulse border rounded-lg p-4 shadow-sm bg-white ${width}`}
          >
            <div className={`bg-gray-200 rounded mb-4 ${height}`} />
            <div className="space-y-2">
              <div className="h-4 bg-gray-200 rounded w-3/4" />
              <div className="h-4 bg-gray-200 rounded w-1/2" />
              <div className="h-4 bg-gray-200 rounded w-full" />
            </div>
          </div>
        );
    }
  };

  const containerClass =
    type === "card"
      ? `grid grid-cols-1 sm:grid-cols-2 md:grid-cols-${columns} gap-4 ${className}`
      : `flex flex-col gap-4 ${className}`;

  return (
    <div className={containerClass}>
      {Array.from({ length: count }).map((_, i) => renderShimmer(i))}
    </div>
  );
}
