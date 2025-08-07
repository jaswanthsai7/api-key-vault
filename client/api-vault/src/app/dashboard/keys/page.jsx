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
import MultiShimmer from "@/components/MultiShimmer";
import TableShimmer from "@/components/TableShimmer";
import ConfirmDialog from "@/components/ConfirmDialog";

export default function ApiKeyManager() {
  const [activeTab, setActiveTab] = useState("keys");
  const [apiKeys, setApiKeys] = useState([]);
  const [scopes, setScopes] = useState([]);
  const [loadingKeys, setLoadingKeys] = useState(false);
  const [loadingScopes, setLoadingScopes] = useState(false);
  const [copiedId, setCopiedId] = useState(null);
  const [showConfirm, setShowConfirm] = useState(false);
  const [selectedKeyId, setSelectedKeyId] = useState(null);

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
    setLoadingKeys(true);
    try {
      const newKey = await generateApiKey();
      toast.success("API Key created");
      setApiKeys([newKey, ...apiKeys]);
    } catch {
      toast.error("Failed to generate key");
    }
    setLoadingKeys(false);
  };

  const handleAskRevoke = (id) => {
    setSelectedKeyId(id);
    setShowConfirm(true);
  };

  const handleConfirmRevoke = async () => {
    setShowConfirm(false);
    try {
      const success = await revokeApiKey(selectedKeyId);
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
              className="bg-black text-white px-4 py-2 rounded hover:bg-gray-800 text-sm"
            >
              Generate New Key
            </button>
          </div>

          {loadingKeys ? (
            <MultiShimmer
              count={4}
              type="card" // card | list | line
              columns={4}
              width="w-full"
              height="h-10"
            />
          ) : apiKeys.length === 0 ? (
            <p className="text-gray-500">No keys found.</p>
          ) : (
            <div className="grid md:grid-cols-2 gap-4">
              {apiKeys.map((key) => (
                <div
                  key={key.id}
                  className="border border-border rounded-lg p-4 bg-white shadow-sm relative"
                >
                  <div className="flex justify-between items-center">
                    <p className="font-mono text-sm truncate text-gray-800">
                      {key.key.slice(0, 6)}...{key.key.slice(-6)}
                    </p>
                    <button
                      onClick={() => {
                        navigator.clipboard.writeText(key.key);
                        setCopiedId(key.id);
                        setTimeout(() => setCopiedId(null), 2000);
                      }}
                      className="text-xs text-gray-600 hover:underline ml-2"
                    >
                      {copiedId === key.id ? "Copied!" : "Copy"}
                    </button>
                  </div>

                  <p className="text-sm text-gray-500 mt-1">
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
                      onClick={() => handleAskRevoke(key.id)}
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
            <TableShimmer
              rows={2}
              columns={3}
              columnWidth="w-full"
              height="h-10"
            />
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

      <ConfirmDialog
        open={showConfirm}
        title="Revoke API Key"
        description="Are you sure you want to revoke this API key? This action cannot be undone."
        onCancel={() => setShowConfirm(false)}
        onConfirm={handleConfirmRevoke}
      />
    </section>
  );
}
