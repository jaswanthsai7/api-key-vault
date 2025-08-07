// /app/not-found.js
import Link from "next/link";

export default function NotFound() {
  return (
    <section className="flex items-center justify-center min-h-screen text-center px-4">
      <div>
        <h1 className="text-5xl font-bold text-gray-800 mb-4">404</h1>
        <p className="text-lg text-gray-600 mb-4">Oops! Page not found.</p>
        <Link
          href="/"
          className="inline-block bg-black text-white px-4 py-2 rounded hover:bg-gray-900"
        >
          Go Home
        </Link>
      </div>
    </section>
  );
}
