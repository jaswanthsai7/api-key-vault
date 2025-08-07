"use client";

import React from "react";

export default function TableShimmer({
  rows = 5,
  columns = 3,
  columnWidth = "w-1/3", // tailwind width class
  height = "h-4",
  gap = "gap-4",
  className = "",
}) {
  return (
    <div
      className={`bg-white rounded-xl shadow overflow-hidden border divide-y divide-gray-100 ${className}`}
    >
      {Array.from({ length: rows }).map((_, rowIndex) => (
        <div
          key={rowIndex}
          className={`animate-pulse flex px-6 py-4 ${gap}`}
        >
          {Array.from({ length: columns }).map((_, colIndex) => (
            <div
              key={colIndex}
              className={`${height} bg-gray-200 rounded ${columnWidth}`}
            />
          ))}
        </div>
      ))}
    </div>
  );
}
