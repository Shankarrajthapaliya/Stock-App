import { useState, useEffect } from "react";

export default function CommentList() {
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadComments() {
      try {
        setLoading(true);
        setError("");

        const response = await fetch("/api/comment");

        if (!response.ok) {
          throw new Error(
            `API not working ${response.status} ${response.statusText}`
          );
        }

        const data = await response.json();
        setComments(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Unknown error");
      } finally {
        setLoading(false);
      }
    }

    loadComments();
  }, []);

  if (loading) return <p>Loading comments...</p>;
  if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

  return (
    <div style={{ padding: 16 }}>
      <h1>Comments</h1>

      {comments.length === 0 ? (
        <p>No comments found.</p>
      ) : (
        <table cellPadding="8" style={{ borderCollapse: "collapse" }}>
          <thead>
            <tr>
              <th align="left">Title</th>
              <th align="left">Content</th>
              <th align="left">Stocks</th>
            </tr>
          </thead>

          <tbody>
            {comments.map((c, idx) => (
              <tr key={idx}>
                <td>{c.title}</td>
                <td>{c.content}</td>
                <td>
                  {c.stock && c.stock.length > 0 ? (
                    <ul>
                      {c.stock.map((s) => (
                        <li key={s.id ?? s.symbol}>
                          {s.companyName}
                        </li>
                      ))}
                    </ul>
                  ) : (
                    <em>No stock</em>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
