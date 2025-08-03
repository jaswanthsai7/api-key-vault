"use client";

import { getAllGroups, getAllRoles, updateUser } from "@/app/lib/userService";
import { useEffect, useState } from "react";

export default function EditUserModal({ user, onClose, onSave }) {
  const [groups, setGroups] = useState([]);
  const [roles, setRoles] = useState([]);
  const [groupId, setGroupId] = useState("");
  const [roleId, setRoleId] = useState("");
  const [loading, setLoading] = useState(false);

  // Load dropdown data and set initial values
  useEffect(() => {
    const loadData = async () => {
      const [groupsRes, rolesRes] = await Promise.all([getAllGroups(), getAllRoles()]);
      setGroups(groupsRes);
      setRoles(rolesRes);

      // Set initial values after data is loaded
      setGroupId(user.group?.id || "");
      setRoleId(user.role?.id || "");
    };

    loadData();
  }, [user]);

  const handleSubmit = async () => {
    if (!groupId || !roleId) return alert("Please select both group and role");

    setLoading(true);
    const result = await updateUser(user.id, {
      email: user.email,
      groupId,
      roleId,
    });
    setLoading(false);
    if (result) onSave(result);
  };

  return (
    <div className="fixed inset-0 z-50 bg-black/40 flex items-center justify-center">
      <div className="bg-white rounded-lg p-6 w-full max-w-md shadow-lg">
        <h2 className="text-xl font-semibold mb-4">Edit User</h2>

        <div className="mb-4">
          <label className="block text-sm font-medium">Email</label>
          <input
            type="text"
            value={user.email}
            readOnly
            className="w-full px-4 py-2 mt-1 border rounded bg-gray-100"
          />
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium">Current Group</label>
          <div className="text-gray-700 mb-1">{user.group?.name || "None"}</div>
          <select
            className="w-full border rounded px-4 py-2 mt-1"
            value={groupId}
            onChange={(e) => setGroupId(e.target.value)}
          >
            <option value="">Select Group</option>
            {groups.map((g) => (
              <option key={g.id} value={g.id}>
                {g.name}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium">Current Role</label>
          <div className="text-gray-700 mb-1">{user.role?.name || "None"}</div>
          <select
            className="w-full border rounded px-4 py-2 mt-1"
            value={roleId}
            onChange={(e) => setRoleId(e.target.value)}
          >
            <option value="">Select Role</option>
            {roles.map((r) => (
              <option key={r.id} value={r.id}>
                {r.name}
              </option>
            ))}
          </select>
        </div>

        <div className="flex justify-end gap-3 mt-6">
          <button className="bg-gray-300 px-4 py-2 rounded" onClick={onClose}>
            Cancel
          </button>
          <button
            className="bg-black text-white px-4 py-2 rounded"
            onClick={handleSubmit}
            disabled={loading}
          >
            {loading ? "Saving..." : "Save"}
          </button>
        </div>
      </div>
    </div>
  );
}
