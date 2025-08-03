"use client";

import { useTheme } from "@/context/ThemeContext";

export default function ThemeSwitcher() {
  const { theme, setTheme } = useTheme();

  return (
    <select
      value={theme}
      onChange={(e) => setTheme(e.target.value)}
      className="px-2 py-1 rounded-md border border-gray-300 text-sm bg-white"
    >
      <option value="lightGray">Light Gray</option>
      <option value="lightBlue">Light Blue</option>
      <option value="lightViolet">Light Violet</option>
      <option value="lightGreen">Light Green</option>
    </select>
  );
}
