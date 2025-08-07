import api from "./api";

export async function loginUser(credentials) {
  const response = await api.post("/Auth/Login", credentials);
  return response.data;
}

export async function registerUser(userInfo) {
  const response = await api.post("/Auth/Register", userInfo);
  return response.data;
}

export async function verifyUser() {
  const response = await api.get("/Auth/Me");
  return response.data;
}

export async function logoutUser() {
  const response = await api.post("/Auth/Logout");
  return response.data;
}
