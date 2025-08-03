import DashboardHeader from "@/components/DashboardHeader";

export default function DashboardHome() {
  return (
    <>
      <DashboardHeader title="Dashboard Overview" />
      <p className="text-gray-700 text-sm">
        Welcome to your dashboard. Here you’ll find key usage stats and manage your API keys.
      </p>
    </>
  );
}
