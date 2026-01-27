import { useMemo, useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";

export default function CommentList() {
  const { symbol } = useParams();
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [query, setQuery] = useState("");

  useEffect(() => {
    async function loadComments() {
      try {
        setLoading(true);
        setError("");

        const response = await fetch(`/api/comment/symbol/${symbol}`);
        if (!response.ok) {
          throw new Error(`API not working ${response.status} ${response.statusText}`);
        }

        const data = await response.json();
        setComments(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Unknown error");
      } finally {
        setLoading(false);
      }
    }

    if (symbol) loadComments();
  }, [symbol]);

  const filteredComments = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return comments;
    return comments.filter((c) => {
      const hay = [c.title, c.content].filter(Boolean).join(" ").toLowerCase();
      return hay.includes(q);
    });
  }, [comments, query]);

  if (loading) {
    return (
      <div className="container">
        <div className="shell">
          <div className="content">
            <p className="muted">Loading comments for {symbol}…</p>
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
          <h1>Comments</h1>
          <p className="muted" style={{ marginTop: 6 }}>
            Notes for <strong style={{ color: "rgba(255,255,255,0.92)" }}>{symbol}</strong>
          </p>
        </div>

        <div className="toolbar">
          <div className="controls">
            <Link className="btn" to="/">
              ← Back
            </Link>

            <input
              className="input"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              placeholder="Search title or content…"
              aria-label="Search comments"
            />

            {query && (
              <button className="btn" onClick={() => setQuery("")}>
                Clear
              </button>
            )}
          </div>

          <div className="muted" style={{ fontSize: 12 }}>
            Showing <strong style={{ color: "rgba(255,255,255,0.92)" }}>{filteredComments.length}</strong> of{" "}
            <strong style={{ color: "rgba(255,255,255,0.92)" }}>{comments.length}</strong>
          </div>
        </div>

        <div className="content">
          {filteredComments.length === 0 ? (
            <div className="empty">No comments found.</div>
          ) : (
            <div className="cardList">
              {filteredComments.map((c) => (
                <div className="card" key={c.id}>
                  <div className="cardTitle">
                    <h3>{c.title}</h3>
                    {c.createdOn ? <span>{new Date(c.createdOn).toLocaleDateString()}</span> : <span />}
                  </div>
                  <p>{c.content}</p>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
