"use client";

import { usePathname } from "next/navigation";
import DashboardSidebar from "@/components/DashboardSidebar";
import Navbar from "@/components/Navbar";
import { ThemeProvider } from "@/context/ThemeContext";
import "./globals.css";



export default function RootLayout({ children }) {
  const pathname = usePathname();

  // Define paths that should NOT show sidebar
  const noSidebarRoutes = ["/login", "/register"];

  const showSidebar = !noSidebarRoutes.includes(pathname);

  return (
    <html lang="en">
      <body className="bg-bg text-text">
        <ThemeProvider>
          {/* Always show Navbar */}
          <Navbar />

          <div className="pt-14">
            {showSidebar && <DashboardSidebar />}

            {/* Main content */}
            <div className={showSidebar ? "md:ml-64 p-4" : "p-4"}>
              {children}
            </div>
          </div>
        </ThemeProvider>
      </body>
    </html>
  );
}
