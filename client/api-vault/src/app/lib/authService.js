import api from "./api";

export async function loginUser(credentials) {
  const response = await api.post("/Auth/login", credentials);
  return response.data;
}

export async function registerUser(userInfo) {
  const response = await api.post("/Auth/register", userInfo);
  return response.data;
}

export async function verifyUser() {
  const response = await api.post("/Auth/Me");
  return response.data;
}
