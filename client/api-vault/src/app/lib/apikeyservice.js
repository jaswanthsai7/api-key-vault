import api from "./api";

// Get all API keys for the logged-in user
export async function getUserApiKeys() {
  const response = await api.get("/apikey/my");
  return response.data;
}

// Generate a new API key
export async function generateApiKey() {
  const response = await api.post("/apikey/generate");
  return response.data.apiKey; // assuming `{ apiKey: { ... } }`
}

// Revoke an existing API key
export async function revokeApiKey(apiKeyId) {
  const response = await api.put(`/apikey/revoke/${apiKeyId}`);
  return response.status === 204;
}

// Get all API scopes for the logged-in user
export async function getUserApiScopes() {
  const response = await api.get("/apikey/scopes");
  return response.data;
}
