import DashboardSidebar from "@/components/DashboardSidebar";

export default function AdminLayout({ children }) {
  return (
    <div className="flex min-h-screen">
      <main className="flex-1 px-8 py-10 bg-accent overflow-y-auto">{children}</main>
    </div>
  );
}
