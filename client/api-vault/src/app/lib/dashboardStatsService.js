import api from "./api";

// Get all API keys for the logged-in user
export async function getDashboardStats() {
  const response = await api.get("/auditlog/stats");
  return response.data;
}