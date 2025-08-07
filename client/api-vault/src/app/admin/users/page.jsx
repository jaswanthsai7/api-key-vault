"use client";

import { useEffect, useState } from "react";
import EditUserModal from "./EditUserModal";
import { getAllUsers } from "@/app/lib/userService";
import TableShimmer from "@/components/TableShimmer";
import MultiShimmer from "@/components/MultiShimmer";

export default function ManageUsersPage() {
  const [users, setUsers] = useState([]);
  const [editingUser, setEditingUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    setLoading(true);
    try {
      const data = await getAllUsers();
      setUsers(data);
    } catch (error) {
      console.error("Failed to load users", error);
    }
    setLoading(false);
  };

  const handleUserUpdated = (updatedUser) => {
    setUsers(prev =>
      prev.map(u => (u.id === updatedUser.id ? updatedUser : u))
    );
    setEditingUser(null);
  };

  return (
    <div className="p-4 sm:p-6">
      <h1 className="text-2xl font-bold mb-4">Manage Users</h1>

      {/* Desktop View */}
      <div className="hidden sm:block overflow-x-auto">
        {loading ? (
          <TableShimmer rows={5} columns={4} height="h-5" columnWidth="w-1/3" />
        ) : (
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
        )}
      </div>

      {/* Mobile View */}
      <div className="sm:hidden space-y-4">
        {loading ? (
          <MultiShimmer
            count={3}
            type="card"
            columns={1}
            height="h-4"
            width="w-full"
          />
        ) : (
          users.map(user => (
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
          ))
        )}
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
