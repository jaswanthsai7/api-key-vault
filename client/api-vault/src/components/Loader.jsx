// components/Loader.jsx
import { Loader } from "lucide-react";
export default function PageLoader({ className = "" }) {
  return (
    <div className="flex justify-center">
      <Loader className={className} />
    </div>
  );
}
