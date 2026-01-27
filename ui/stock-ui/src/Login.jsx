import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./auth/authContext";

export default function Login() {
  const nav = useNavigate();
  const { login } = useAuth();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  // These control UI states
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  async function onSubmit(e) {
    e.preventDefault(); // stops the browser from refreshing the page
    setLoading(true);
    setError("");

    try {
      const res = await fetch("/api/auth/login", {
        method: "POST", // we are sending data, so POST
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }), // backend expects these fields
      });

      if (!res.ok) {
        const msg = await res.json().catch(() => null);
        throw new Error(msg?.message || `Login failed (${res.status})`);
      }

      const data = await res.json();
      // data is like: { username, token, expiration }

      login(data);       // store token in our auth system
      nav("/", { replace: true }); // go to home page after login
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unknown error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={{ padding: 16 }}>
      <h1>Login</h1>

      <form onSubmit={onSubmit} style={{ display: "grid", gap: 10, maxWidth: 320 }}>
        <input
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          placeholder="Username"
        />

        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Password"
        />

        {error && <div style={{ color: "red" }}>{error}</div>}

        <button disabled={loading}>
          {loading ? "Signing in..." : "Sign in"}
        </button>
      </form>
    </div>
  );
}
