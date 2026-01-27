import { getAuth, clearAuth } from "./auth/authStorage";

export async function apiFetch(path, options = {}) {
  const auth = getAuth();
  const headers = new Headers(options.headers || {});

  if (options.body && !headers.has("Content-Type")) {
    headers.set("Content-Type", "application/json");
  }

  if (auth?.token) {
    headers.set("Authorization", `Bearer ${auth.token}`);
  }

  const res = await fetch(path, { ...options, headers });

  if (res.status === 401) {
    clearAuth(); // token invalid/expired
  }

  return res;
}
