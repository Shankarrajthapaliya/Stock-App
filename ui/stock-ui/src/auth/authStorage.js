const KEY = "auth";

// read saved auth from browser
export function getAuth() {
  const raw = localStorage.getItem(KEY); // get string from browser
  if (!raw) return null;

  try {
    return JSON.parse(raw); // convert string -> object
  } catch {
    return null;
  }
}

// save auth to browser
export function setAuth(auth) {
  localStorage.setItem(KEY, JSON.stringify(auth));
}

// delete auth
export function clearAuth() {
  localStorage.removeItem(KEY);
}
