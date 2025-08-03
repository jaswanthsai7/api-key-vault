import DashboardHeader from "@/components/DashboardHeader";

export default function UsagePage() {
  return (
    <>
      <DashboardHeader title="Usage & Logs" />
      
      {/* Usage Chart Placeholder */}
      <div className="bg-white p-6 rounded-lg border border-gray-200 shadow-sm mb-6">
        <p className="font-medium text-gray-700 mb-2">API Requests (Past 7 Days)</p>
        <div className="h-40 bg-accent rounded-md flex items-center justify-center text-sm text-gray-500">
          [Chart coming soon...]
        </div>
      </div>

      {/* Logs Table */}
      <div className="bg-white p-6 rounded-lg border border-gray-200 shadow-sm">
        <p className="font-medium text-gray-700 mb-4">Recent API Logs</p>
        <table className="w-full text-sm text-left text-gray-700">
          <thead>
            <tr>
              <th className="pb-2">Time</th>
              <th className="pb-2">Endpoint</th>
              <th className="pb-2">Status</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td className="py-1">12:34 PM</td>
              <td>/v1/user/profile</td>
              <td>200 OK</td>
            </tr>
            <tr>
              <td className="py-1">12:30 PM</td>
              <td>/v1/data/stats</td>
              <td>401 Unauthorized</td>
            </tr>
          </tbody>
        </table>
      </div>
    </>
  );
}
