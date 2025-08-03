"use client";

import { useState } from "react";
import axios from "axios";
import { Trash2 } from "lucide-react"; // optional icon if using lucide-react

export default function TestApiPage() {
  const [url, setUrl] = useState("");
  const [method, setMethod] = useState("GET");
  const [headers, setHeaders] = useState([{ key: "", value: "" }]);
  const [body, setBody] = useState("");
  const [response, setResponse] = useState(null);
  const [error, setError] = useState(null);

  const handleHeaderChange = (index, field, value) => {
    const updated = [...headers];
    updated[index][field] = value;
    setHeaders(updated);
  };

  const addHeader = () => setHeaders([...headers, { key: "", value: "" }]);

  const deleteHeader = (index) => {
    const updated = [...headers];
    updated.splice(index, 1);
    setHeaders(updated);
  };

  const sendRequest = async () => {
    try {
      setError(null);

      const token = localStorage.getItem("token"); // adjust key if needed
      const addCustomToken=headers.reduce((acc, { key, value }) => {
          if (key.trim()) acc[key] = value;
          return acc;
        }, {})

        if(token){
          addCustomToken["AuthID"]=token;
        }

      const config = {
        method,
        url,
        headers: addCustomToken ,
      };

      if (["POST", "PUT", "PATCH"].includes(method.toUpperCase()) && body.trim()) {
        config.data = JSON.parse(body);
      }

      const res = await axios(config);
      setResponse({
        status: res.status,
        headers: res.headers,
        data: res.data,
      });
    } catch (err) {
      setResponse(null);
      setError(err.response || err.message || "Unknown error");
    }
  };

  return (
    <div className="max-w-4xl mx-auto py-10 px-6">
      <h1 className="text-2xl font-bold mb-6">Verify Your API Key</h1>

      <div className="space-y-4 bg-white p-6 shadow-md rounded-lg border">
        <div className="flex gap-2">
          <select
            value={method}
            onChange={(e) => setMethod(e.target.value)}
            className="border p-2 rounded w-28"
          >
            {["GET", "POST", "PUT", "DELETE", "PATCH"].map((m) => (
              <option key={m}>{m}</option>
            ))}
          </select>
          <input
            type="text"
            placeholder="https://your-api-url.com/endpoint"
            value={url}
            onChange={(e) => setUrl(e.target.value)}
            className="flex-1 border p-2 rounded"
          />
        </div>

        <div>
          <div className="flex justify-between items-center mb-1">
            <h2 className="font-semibold text-sm">Headers</h2>
            <button
              onClick={addHeader}
              className="bg-black text-white text-xs px-3 py-1 rounded hover:bg-gray-900"
            >
              + Add Header
            </button>
          </div>

          {headers.map((header, index) => (
            <div className="flex gap-2 items-center mb-2" key={index}>
              <input
                type="text"
                placeholder="Key"
                className="w-1/2 border p-2 rounded"
                value={header.key}
                onChange={(e) => handleHeaderChange(index, "key", e.target.value)}
              />
              <input
                type="text"
                placeholder="Value"
                className="w-1/2 border p-2 rounded"
                value={header.value}
                onChange={(e) => handleHeaderChange(index, "value", e.target.value)}
              />
              <button
                onClick={() => deleteHeader(index)}
                className="text-red-600 text-xs hover:underline"
              >
                Delete
              </button>
              {/* Or use an icon:
              <button onClick={() => deleteHeader(index)} className="text-red-500">
                <Trash2 size={16} />
              </button> */}
            </div>
          ))}
        </div>

        {["POST", "PUT", "PATCH"].includes(method.toUpperCase()) && (
          <div>
            <h2 className="font-semibold text-sm mb-1">Request Body (JSON)</h2>
            <textarea
              value={body}
              onChange={(e) => setBody(e.target.value)}
              rows={6}
              className="w-full border p-2 font-mono rounded"
              placeholder='{"example":"value"}'
            ></textarea>
          </div>
        )}

        <button
          onClick={sendRequest}
          className="bg-black hover:bg-gray-900 text-white px-4 py-2 rounded text-sm font-semibold"
        >
          Send Request
        </button>
      </div>

      {/* Response Section */}
      {response && (
        <div className="mt-6 bg-gray-100 p-4 rounded-lg border text-sm">
          <h3 className="font-bold mb-2">Response</h3>
          <div>Status: {response.status}</div>
          <div className="mt-2">
            <h4 className="font-semibold">Headers:</h4>
            <pre className="overflow-x-auto">{JSON.stringify(response.headers, null, 2)}</pre>
          </div>
          <div className="mt-2">
            <h4 className="font-semibold">Body:</h4>
            <pre className="overflow-x-auto">{JSON.stringify(response.data, null, 2)}</pre>
          </div>
        </div>
      )}

      {error && (
        <div className="mt-6 text-sm text-red-600 bg-red-50 p-4 rounded border border-red-200">
          <h3 className="font-bold">Error</h3>
          <pre>{JSON.stringify(error, null, 2)}</pre>
        </div>
      )}
    </div>
  );
}
