export default function Home() {
  return (
    <section className="min-h-screen bg-accent px-4 md:px-6 pt-12 md:pt-20 overflow-hidden">
      <div className="max-w-7xl w-full grid grid-cols-1 md:grid-cols-2 gap-12 items-start">
        {/* Text Content */}
        <div>
          <h1 className="text-4xl sm:text-5xl md:text-6xl font-bold text-gray-900 leading-tight mb-6">
            Secure. Monitor. Scale.
            <br />
            <span className="text-gray-700">API Vault for Developers</span>
          </h1>
          <p className="text-base sm:text-lg text-gray-600 mb-8 max-w-xl">
            Effortlessly manage API keys with scopes, real-time usage analytics, logging, and rate limiting.
            Built for developers who care about control, security, and insights — without the overhead.
          </p>
          <div className="flex flex-wrap items-center gap-4">
            <a
              href="/register"
              className="bg-black hover:bg-gray-900 text-white px-6 py-3 rounded-lg text-base font-medium transition"
            >
              Get Started
            </a>
            <a href="/login" className="flex items-center gap-1 text-base font-medium text-gray-700 hover:underline">
              <span>Already have an account?</span>
              <span className="font-semibold text-gray-900">Login</span>
            </a>
          </div>
        </div>

        {/* Code block (visible on all screens now, scrollable on mobile) */}
        <div className="w-full mt-5">
          <div className="bg-white shadow-xl rounded-xl p-4 md:p-6 border border-gray-200 max-w-full overflow-x-auto">
            <pre className="text-sm text-gray-800 whitespace-pre-wrap">
{`POST /api/keys
Authorization: Bearer <token>

{
  "name": "prod-key",
  "scopes": ["read", "write"]
}`}
            </pre>
            
          </div>
          <div className="bg-white shadow-xl rounded-xl p-4 md:p-6 border border-gray-200 max-w-full overflow-x-auto mt-4">
            <pre className="text-sm text-gray-800 whitespace-pre-wrap">
{`POST /api/keys
Authorization: Bearer <token>

{
  "name": "prod-key",
  "scopes": ["read", "write"]
}`}
            </pre>
            
          </div>
        </div>
      </div>
    </section>
  );
}
