import { createContext, useContext, useMemo, useState } from "react";
import { clearAuth, getAuth, setAuth } from "./authStorage";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  // Start by loading auth from localStorage (so refresh keeps login)
  const [auth, setAuthState] = useState(() => getAuth());

  const value = useMemo(() => {
    const token = auth?.token ?? null;

    return {
      auth,
      token,
      isAuthed: !!token, // true if token exists, false if not

      login: (payload) => {
        setAuth(payload);        // save to localStorage
        setAuthState(payload);   // save to React memory
      },

      logout: () => {
        clearAuth();             // remove from localStorage
        setAuthState(null);      // remove from React memory
      },
    };
  }, [auth]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
