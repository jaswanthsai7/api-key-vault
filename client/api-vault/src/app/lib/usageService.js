import api from "./api";

// Get recent audit logs for logged-in user
export async function getUserAuditLogs() {
  const response = await api.get("/AuditLog/my");
  return response.data;
}

// Get user-specific API usage stats (for chart)
export async function getUserUsageStats() {
  const response = await api.get("/AuditLog/stats");
  return response.data;
}
