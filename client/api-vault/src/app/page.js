"use client";

import Link from "next/link";
import { apiResponseJson } from "./constants/api-json";
import { useEffect, useRef } from "react";

export default function Home() {
  const scrollRef = useRef(null);

  useEffect(() => {
    const el = scrollRef.current;
    if (!el) return;

    const firstChild = el.children[0];
    const clone = firstChild.cloneNode(true);
    el.appendChild(clone); // duplicate content to enable loop

    let y = 0;
    let animationFrameId;

    const scroll = () => {
      y += 0.5; // Speed
      if (y >= firstChild.scrollHeight) {
        y = 0; // Reset scroll
      }

      el.scrollTop = y;
      animationFrameId = requestAnimationFrame(scroll);
    };

    animationFrameId = requestAnimationFrame(scroll);

    return () => {
      cancelAnimationFrame(animationFrameId);
      if (el.lastChild === clone) {
        el.removeChild(clone);
      }
    };
  }, []);
  return (
    <section className="min-h-screen bg-accent px-4 md:px-6  overflow-hidden">
      <div className="max-w-7xl w-full grid grid-cols-1 md:grid-cols-2 gap-12 items-start">
        {/* Text Content */}
        <div>
          <h1 className="text-4xl sm:text-5xl md:text-6xl pt-12 md:pt-20 font-bold text-gray-900 leading-tight mb-6">
            Secure. Monitor. Scale.
            <br />
            <span className="text-gray-700">API Vault for Developers</span>
          </h1>
          <p className="text-base sm:text-lg text-gray-600 mb-8 max-w-xl">
            Effortlessly manage API keys with scopes, real-time usage analytics,
            logging, and rate limiting. Built for developers who care about
            control, security, and insights — without the overhead.
          </p>
          <div className="flex flex-wrap items-center gap-4">
            <a
              href="/register"
              className="bg-black hover:bg-gray-900 text-white px-6 py-3 rounded-lg text-base font-medium transition"
            >
              Get Started
            </a>
            <Link
              href="/login"
              className="flex items-center gap-1 text-base font-medium text-gray-700"
            >
              <span>Already have an account?</span>
              <span className="text-gray-900 font-semibold hover:underline">
                Login
              </span>
            </Link>
          </div>
        </div>

        {/* Right: Vertical Marquee */}
        <div className="w-full mt-5 h-[650px] overflow-hidden">
          <div
            ref={scrollRef}
            className="h-full overflow-hidden hide-scrollbar"
          >
            <div className="space-y-4">
              {/* Repeat content twice for seamless loop */}
              {[...apiResponseJson, ...apiResponseJson].map((block, i) => (
                <div
                  key={i}
                  className="bg-white rounded-xl p-4 md:p-6 pt-2 border border-gray-200 max-w-full overflow-x-auto"
                >
                  <pre className="text-sm text-gray-800 whitespace-pre-wrap">
                    {block.code}
                  </pre>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
