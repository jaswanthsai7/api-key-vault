import DashboardHeader from "@/components/DashboardHeader";

export default function AccountSettingsPage() {
  return (
    <>
      <DashboardHeader title="Account Settings" />
      <form className="space-y-6 max-w-lg bg-white p-6 rounded-lg border border-gray-200 shadow-sm">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
          <input
            type="email"
            defaultValue="user@example.com"
            className="w-full px-4 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-black"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Change Password</label>
          <input
            type="password"
            placeholder="New password"
            className="w-full px-4 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-black"
          />
        </div>

        <button
          type="submit"
          className="bg-black text-white px-6 py-2 rounded-lg text-sm font-medium hover:bg-gray-900"
        >
          Save Changes
        </button>
      </form>
    </>
  );
}
