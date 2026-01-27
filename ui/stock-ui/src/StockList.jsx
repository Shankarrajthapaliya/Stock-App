import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";

export default function StockList() {
  const [stocks, setStocks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [query, setQuery] = useState("");

  useEffect(() => {
    async function loadStocks() {
      try {
        setError("");
        setLoading(true);

        const response = await fetch("/api/stock");
        if (!response.ok) {
          throw new Error(`Api not found ${response.status} ${response.statusText}`);
        }

        const data = await response.json();
        setStocks(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Unknown Error");
      } finally {
        setLoading(false);
      }
    }

    loadStocks();
  }, []);

  const filteredStocks = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return stocks;

    return stocks.filter((s) => {
      const haystack = [
        s.symbol,
        s.companyName,
        s.industry,
      ]
        .filter(Boolean)
        .join(" ")
        .toLowerCase();

      return haystack.includes(q);
    });
  }, [stocks, query]);

  if (loading) {
    return (
      <div className="container">
        <div className="shell">
          <div className="content">
            <p className="muted">Loading stocks…</p>
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container">
        <div className="shell">
          <div className="content">
            <p style={{ color: "#ef4444" }}>Error: {error}</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container">
      <div className="shell">
        <div className="header">
          <h1>Stocks</h1>
        </div>

        <div className="toolbar">
          <div className="controls">
            <input
              className="input"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              placeholder="Search symbol, company, industry…"
              aria-label="Search stocks"
            />

            {query && (
              <button className="btn" onClick={() => setQuery("")}>
                Clear
              </button>
            )}
          </div>

          <div className="muted" style={{ fontSize: 12 }}>
            Showing <strong style={{ color: "rgba(255,255,255,0.92)" }}>{filteredStocks.length}</strong> of{" "}
            <strong style={{ color: "rgba(255,255,255,0.92)" }}>{stocks.length}</strong>
          </div>
        </div>

        <div className="content">
          {filteredStocks.length === 0 ? (
            <div className="empty">No stocks found.</div>
          ) : (
            <div className="tableWrap">
              <table className="table" cellPadding="0" cellSpacing="0">
                <thead>
                  <tr>
                    <th style={{ width: 120 }}>Symbol</th>
                    <th style={{ width: 300 }}>Company</th>
                    <th style={{ width: 180 }}>Industry</th>
                    <th>Highlights</th>
                  </tr>
                </thead>

                <tbody>
                  {filteredStocks.map((s) => {
                    const comments = Array.isArray(s.comments) ? s.comments : [];
                    const chips = comments.slice(0, 3);

                    return (
                      <tr className="row" key={s.id ?? s.symbol}>
                        <td>
                          <Link className="symbolLink" to={`/stocks/${s.symbol}/comments`}>
                            {s.symbol}
                          </Link>
                        </td>

                        <td>
                          <div style={{ fontWeight: 700 }}>{s.companyName}</div>
                          <div className="muted" style={{ fontSize: 12, marginTop: 4 }}>
                            {comments.length ? `${comments.length} comment${comments.length === 1 ? "" : "s"}` : "No comments yet"}
                          </div>
                        </td>

                        <td>
                          <span className="pill">{s.industry || "—"}</span>
                        </td>

                        <td>
                          {comments.length > 0 ? (
                            <>
                              <div className="chips">
                                {chips.map((c) => (
                                  <span className="chip" key={c.id ?? `${c.title}-${c.createdOn ?? ""}`} title={c.content}>
                                    {c.title}
                                  </span>
                                ))}
                                {comments.length > 3 && (
                                  <span className="chip" title="More comments available">
                                    +{comments.length - 3} more
                                  </span>
                                )}
                              </div>

                              <div style={{ marginTop: 10 }}>
                                <Link className="muted" to={`/stocks/${s.symbol}/comments`} style={{ fontSize: 12 }}>
                                  View all comments →
                                </Link>
                              </div>
                            </>
                          ) : (
                            <span className="muted">—</span>
                          )}
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
