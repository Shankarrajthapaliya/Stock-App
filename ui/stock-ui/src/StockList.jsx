import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

export default function StockList() {
  const [stocks, setStocks] = useState([]);   
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

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

  if (loading) return <p>Loading stocks...</p>;
  if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

  return (
    <div style={{ padding: 16 }}>
      <h1>Stocks</h1>

      {stocks.length === 0 ? (
        <p>No stocks found.</p>
      ) : (
        <table cellPadding="8" style={{ borderCollapse: "collapse" }}>
          <thead>
            <tr>
              <th align="left">Symbol</th>
              <th align="left">Company</th>
              <th align="left">Industry</th>
              <th align="left">Comment</th>
            </tr>
          </thead>

          <tbody>
            {stocks.map((s) => (
              <tr key={s.id ?? s.symbol}>
                <td>
                  <Link to={`/stocks/${s.symbol}/comments`}>{s.symbol}</Link>
                </td>
                <td>{s.companyName}</td>
                <td>{s.industry}</td>
                <td>
                  {s.comments && s.comments.length > 0 ? (
                    <ul>
                      {s.comments.map((c) => (
                        <li key={c.id ?? `${c.title}-${c.createdOn ?? ""}`}>
                          <strong>{c.title}</strong>
                          <div>{c.content}</div>
                        </li>
                      ))}
                    </ul>
                  ) : (
                    <em>No comments</em>
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
