"use client";

import { useEffect, useState } from "react";
import {
  getAllGroups,
  getAllRoles,
  updateUser,
} from "@/app/lib/userService";

export default function EditUserModal({ user, onClose, onSave }) {
  const [groups, setGroups] = useState([]);
  const [roles, setRoles] = useState([]);
  const [groupId, setGroupId] = useState("");
  const [roleId, setRoleId] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const loadData = async () => {
      try {
        const [groupsRes, rolesRes] = await Promise.all([
          getAllGroups(),
          getAllRoles(),
        ]);
        setGroups(groupsRes);
        setRoles(rolesRes);

        // Set defaults only after data is fetched
        setGroupId(user.group?.id || "");
        setRoleId(user.role?.id || "");
      } catch (error) {
        console.error("Failed to load roles/groups", error);
      }
    };

    loadData();
  }, [user]);

  const handleSubmit = async () => {
    if (!groupId || !roleId) {
      alert("Please select both group and role");
      return;
    }

    const isUnchanged =
      groupId === user.group?.id && roleId === user.role?.id;
    if (isUnchanged) {
      onClose(); // no update needed
      return;
    }

    try {
      setLoading(true);
      const result = await updateUser(user.id, {
        email: user.email,
        groupId,
        roleId,
      });
      if (result) onSave(result);
    } catch (error) {
      alert("Update failed. Try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center backdrop-blur-sm bg-black/30 transition-opacity">
      <div className="bg-white rounded-lg shadow-xl w-full max-w-md p-6">
        <h2 className="text-xl font-semibold text-gray-900 mb-4">
          Edit User
        </h2>

        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">
              Email
            </label>
            <input
              type="text"
              readOnly
              value={user.email}
              className="mt-1 w-full px-3 py-2 bg-gray-100 border rounded text-sm text-gray-800"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">
              Group
            </label>
            <select
              value={groupId}
              onChange={(e) => setGroupId(e.target.value)}
              className="mt-1 w-full px-3 py-2 border rounded text-sm"
            >
              <option value="">Select Group</option>
              {groups.map((g) => (
                <option key={g.id} value={g.id}>
                  {g.name}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">
              Role
            </label>
            <select
              value={roleId}
              onChange={(e) => setRoleId(e.target.value)}
              className="mt-1 w-full px-3 py-2 border rounded text-sm"
            >
              <option value="">Select Role</option>
              {roles.map((r) => (
                <option key={r.id} value={r.id}>
                  {r.name}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="mt-6 flex justify-end gap-3">
          <button
            className="px-4 py-2 text-sm rounded border text-gray-700 hover:bg-gray-100"
            onClick={onClose}
            disabled={loading}
          >
            Cancel
          </button>
          <button
            className="px-4 py-2 text-sm rounded bg-black text-white hover:bg-gray-900"
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
