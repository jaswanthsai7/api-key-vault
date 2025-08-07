export const apiResponseJson = [
  {
    title: "Create API Key",
    code: `POST /api/keys
Authorization: Bearer <token>

{
  "name": "prod-key",
  "scopes": ["read", "write"]
}`,
  },
  {
    title: "Generate Key",
    code: `POST /api/keys/generate
Authorization: Bearer <token>

{
  "key": "v3_a1b2c3d4e5f6g7h8i9",
  "createdAt": "2025-08-07T14:15:00Z",
  "scopes": ["read", "write"]
}`,
  },
  {
    title: "List My Keys",
    code: `GET /api/keys/my
Authorization: Bearer <token>

[
  {
    "key": "v3_a1b2c3d4e5f6g7h8i9",
    "createdAt": "2025-08-07T14:15:00Z",
    "isRevoked": false
  },
  {
    "key": "v3_x9y8z7w6v5u4t3s2r1",
    "createdAt": "2025-06-21T09:42:00Z",
    "isRevoked": true
  }
]`,
  },
];
