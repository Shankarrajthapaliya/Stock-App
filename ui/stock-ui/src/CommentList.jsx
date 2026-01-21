import { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";

export default function CommentList() {
  const { symbol } = useParams();             
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadComments() {
      try {
        setLoading(true);
        setError("");

        const response = await fetch(`/api/comment/symbol/${symbol}`); 

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

    if (symbol) loadComments();
  }, [symbol]); 

  if (loading) return <p>Loading comments for {symbol}...</p>;
  if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

  return (
    <div style={{ padding: 16 }}>
      <h1>Comments for {symbol}</h1>

      <p>
        <Link to="/">← Back to Stocks</Link>
      </p>

      {comments.length === 0 ? (
        <p>No comments found.</p>
      ) : (
        <table cellPadding="8" style={{ borderCollapse: "collapse" }}>
          <thead>
            <tr>
              <th align="left">Title</th>
              <th align="left">Content</th>
            </tr>
          </thead>

          <tbody>
            {comments.map((c) => (
              <tr key={c.id}>
                <td>{c.title}</td>
                <td>{c.content}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
