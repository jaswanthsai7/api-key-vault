import axios from "axios";

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_BASE || "https://localhost:56323/api",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, // Send cookies (including HTTP-only auth tokens)
});

export default api;
