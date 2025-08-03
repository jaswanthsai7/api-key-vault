import api from "./api";

// Get all users
export async function getAllUsers() {
  const response = await api.get("/User/GetAllUsers");
  return response.data;
}

// Get all groups
export async function getAllGroups() {
  const response = await api.get("/Group/GetAllGroups");
  return response.data;
}

// Get all roles
export async function getAllRoles() {
  const response = await api.get("/Role/GetAllRoles");
  return response.data;
}

// Update a user
export async function updateUser(id, data) {
  try {
    const response = await api.put(`/User/UpdateUser?id=${id}`, data);
    return response.data;
  } catch (error) {
    console.error("Failed to update user:", error);
    return null;
  }
}
