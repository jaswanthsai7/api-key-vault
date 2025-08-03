// src/app/layout.js
import { ThemeProvider } from "@/context/ThemeContext";
import { AuthProvider } from "@/context/AuthContext";
import Navbar from "@/components/Navbar";
import ClientLayout from "@/components/ClientLayout";
import "./globals.css";

export default function RootLayout({ children }) {
  
  return (
    <html lang="en">
      <body className="bg-bg text-text">
        <ThemeProvider>
          <AuthProvider>
            <Navbar />
            <ClientLayout>{children}</ClientLayout>
          </AuthProvider>
        </ThemeProvider>
      </body>
    </html>
  );
}
