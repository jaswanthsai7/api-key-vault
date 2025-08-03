"use client";

import { useState, useEffect } from "react";
import { toast } from "react-hot-toast";
import { formatDistanceToNow } from "date-fns";
import {
  getUserApiScopes,
  getUserApiKeys,
  generateApiKey,
  revokeApiKey,
} from "@/app/lib/apikeyservice";

export default function ApiKeyManager() {
  const [activeTab, setActiveTab] = useState("keys");
  const [apiKeys, setApiKeys] = useState([]);
  const [scopes, setScopes] = useState([]);
  const [loadingKeys, setLoadingKeys] = useState(false);
  const [loadingScopes, setLoadingScopes] = useState(false);

  const fetchKeys = async () => {
    setLoadingKeys(true);
    try {
      const keys = await getUserApiKeys();
      setApiKeys(keys);
    } catch {
      toast.error("Failed to load keys");
    }
    setLoadingKeys(false);
  };

  const fetchScopes = async () => {
    setLoadingScopes(true);
    try {
      const scopes = await getUserApiScopes();
      setScopes(scopes);
    } catch {
      toast.error("Failed to load scopes");
    }
    setLoadingScopes(false);
  };

  const handleGenerateKey = async () => {
    try {
      const newKey = await generateApiKey();
      toast.success("API Key created");
      setApiKeys([newKey, ...apiKeys]);
    } catch {
      toast.error("Failed to generate key");
    }
  };

  const handleRevokeKey = async (id) => {
    try {
      const success = await revokeApiKey(id);
      if (success) {
        toast.success("Key revoked");
        fetchKeys();
      }
    } catch {
      toast.error("Failed to revoke key");
    }
  };

  useEffect(() => {
    fetchKeys();
    fetchScopes();
  }, []);

  return (
    <section className="max-w-5xl mx-auto py-10 px-4 md:px-6 text-foreground">
      <h1 className="text-2xl font-semibold mb-6">API Key Management</h1>

      {/* Tabs */}
      <div className="flex space-x-4 border-b border-border mb-6">
        <button
          onClick={() => setActiveTab("keys")}
          className={`py-2 px-4 text-sm font-medium ${
            activeTab === "keys"
              ? "border-b-2 border-black text-black"
              : "text-gray-600 hover:text-black"
          }`}
        >
          API Keys
        </button>
        <button
          onClick={() => setActiveTab("scopes")}
          className={`py-2 px-4 text-sm font-medium ${
            activeTab === "scopes"
              ? "border-b-2 border-black text-black"
              : "text-gray-600 hover:text-black"
          }`}
        >
          Accessible APIs
        </button>
      </div>

      {/* === API Keys Tab === */}
      {activeTab === "keys" && (
        <div>
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg font-medium text-gray-800">Your Keys</h2>
            <button
              onClick={handleGenerateKey}
              className="bg-gray-900 text-white px-4 py-2 rounded hover:bg-gray-700 text-sm"
            >
              Generate New Key
            </button>
          </div>

          {loadingKeys ? (
            <p className="text-gray-500">Loading keys...</p>
          ) : apiKeys.length === 0 ? (
            <p className="text-gray-500">No keys found.</p>
          ) : (
            <div className="grid md:grid-cols-2 gap-4">
              {apiKeys.map((key) => (
                <div
                  key={key.id}
                  className="border border-border rounded-lg p-4 bg-white shadow-sm"
                >
                  <p className="font-mono text-sm truncate text-gray-800">
                    {key.key.slice(0, 6)}...{key.key.slice(-6)}
                  </p>
                  <p className="text-sm text-gray-500">
                    Created {formatDistanceToNow(new Date(key.createdAt))} ago
                  </p>
                  <p
                    className={`text-sm font-medium mt-1 ${
                      key.isRevoked ? "text-red-600" : "text-green-600"
                    }`}
                  >
                    {key.isRevoked ? "Revoked" : "Active"}
                  </p>

                  {!key.isRevoked && (
                    <button
                      onClick={() => handleRevokeKey(key.id)}
                      className="mt-3 bg-red-600 text-white px-3 py-1 rounded text-sm hover:bg-red-700"
                    >
                      Revoke
                    </button>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>
      )}

      {/* === API Scopes Tab === */}
      {activeTab === "scopes" && (
        <div className="grid md:grid-cols-2 gap-4">
          {loadingScopes ? (
            <p className="text-gray-500">Loading scopes...</p>
          ) : scopes.length === 0 ? (
            <p className="text-gray-500">No API access configured.</p>
          ) : (
            scopes.map((scope, index) => (
              <div
                key={index}
                className="border border-border rounded-lg p-4 bg-white shadow-sm"
              >
                <h3 className="font-semibold text-base text-gray-800">
                  {scope.name}
                </h3>
                <p className="text-sm text-gray-500">{scope.route}</p>
                <p className="text-sm mt-1 text-gray-700">
                  {scope.description}
                </p>
              </div>
            ))
          )}
        </div>
      )}
    </section>
  );
}
