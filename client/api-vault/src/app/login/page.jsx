"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import CryptoJS from "crypto-js";// adjust path if needed
import { loginUser } from "../lib/authService";
import { encryptPassword } from "../lib/encryptPassword";


export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMsg, setErrorMsg] = useState("");
  const router = useRouter();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const encryptedPassword = encryptPassword(password);
      const token = await loginUser({
        email,
        password: encryptedPassword,
      });

      localStorage.setItem("token", token.accessToken);
      router.push("/dashboard");
    } catch (error) {
      const msg =
        error?.response?.data?.message ||
        error?.message ||
        "Login failed. Try again.";
      setErrorMsg(msg);
    }
  };

  return (
    <section className="min-h-screen flex items-center justify-center bg-accent px-4">
      <div className="w-full max-w-md bg-white shadow-xl rounded-xl p-8 border border-gray-200">
        <h2 className="text-2xl font-bold text-gray-900 mb-6 text-center">
          Login to API Vault
        </h2>

        {errorMsg && (
          <div className="text-sm text-red-600 text-center mb-4">{errorMsg}</div>
        )}

        <form className="space-y-5" onSubmit={handleSubmit}>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Email
            </label>
            <input
              type="email"
              required
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-black"
              placeholder="you@example.com"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Password
            </label>
            <input
              type="password"
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-black"
              placeholder="••••••••"
            />
          </div>

          <button
            type="submit"
            className="w-full bg-black hover:bg-gray-900 text-white py-2 rounded-lg text-sm font-semibold transition"
          >
            Sign In
          </button>
        </form>

        <div className="text-center text-sm text-gray-600 mt-6">
          Don’t have an account?{" "}
          <a
            href="/register"
            className="text-gray-900 font-medium hover:underline"
          >
            Register
          </a>
        </div>
      </div>
    </section>
  );
}
