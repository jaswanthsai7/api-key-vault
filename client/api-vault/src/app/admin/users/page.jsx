"use client";

import { useEffect, useState } from "react";
import EditUserModal from "./EditUserModal";
import { getAllUsers } from "@/app/lib/userService";

export default function ManageUsersPage() {
  const [users, setUsers] = useState([]);
  const [editingUser, setEditingUser] = useState(null);

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    const data = await getAllUsers();
    setUsers(data);
  };

  const handleUserUpdated = (updatedUser) => {
    setUsers(prev =>
      prev.map(u => u.id === updatedUser.id ? updatedUser : u)
    );
    setEditingUser(null);
  };

  return (
    <div className="p-4 sm:p-6">
      <h1 className="text-2xl font-bold mb-4">Manage Users</h1>

      {/* Desktop Table View */}
      <div className="hidden sm:block overflow-x-auto">
        <table className="min-w-full bg-white shadow rounded">
          <thead className="bg-gray-100 text-left">
            <tr>
              <th className="p-3">Email</th>
              <th className="p-3">Group</th>
              <th className="p-3">Role</th>
              <th className="p-3">Actions</th>
            </tr>
          </thead>
          <tbody>
            {users.map(user => (
              <tr key={user.id} className="border-t hover:bg-gray-50">
                <td className="p-3">{user.email}</td>
                <td className="p-3">{user.group?.name || "—"}</td>
                <td className="p-3">{user.role?.name || "—"}</td>
                <td className="p-3">
                  <button
                    className="bg-black text-white btn px-4 py-1 rounded"
                    onClick={() => setEditingUser(user)}
                  >
                    Edit
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Mobile Card View */}
      <div className="sm:hidden space-y-4">
        {users.map(user => (
          <div
            key={user.id}
            className="bg-white rounded shadow p-4 border border-gray-200"
          >
            <p><strong>Email:</strong> {user.email}</p>
            <p><strong>Group:</strong> {user.group?.name || "—"}</p>
            <p><strong>Role:</strong> {user.role?.name || "—"}</p>
            <button
              className="mt-3 bg-black text-white btn px-4 py-1 rounded w-full"
              onClick={() => setEditingUser(user)}
            >
              Edit
            </button>
          </div>
        ))}
      </div>

      {editingUser && (
        <EditUserModal
          user={editingUser}
          onClose={() => setEditingUser(null)}
          onSave={handleUserUpdated}
        />
      )}
    </div>
  );
}
