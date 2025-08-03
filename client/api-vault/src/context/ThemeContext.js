"use client";

import { createContext, useContext, useState, useEffect } from "react";

const ThemeContext = createContext();

const themes = {
  lightGray: {
    accent: "#f3f4f6",
    accentHover: "#e5e7eb",
  },
  lightBlue: {
    accent: "#f0f9ff",
    accentHover: "#e0f2fe",
  },
  lightViolet: {
    accent: "#f5f3ff",
    accentHover: "#ede9fe",
  },
  lightGreen: {
    accent: "#ecfdf5",
    accentHover: "#d1fae5",
  },
};

export function ThemeProvider({ children }) {
  const [theme, setTheme] = useState("lightGray");

  useEffect(() => {
    const selected = themes[theme];
    if (selected) {
      document.documentElement.style.setProperty("--color-accent", selected.accent);
      document.documentElement.style.setProperty("--color-accent-hover", selected.accentHover);
    }
  }, [theme]);

  return (
    <ThemeContext.Provider value={{ theme, setTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

export function useTheme() {
  return useContext(ThemeContext);
}
