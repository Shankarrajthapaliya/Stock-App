import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import StockList from "./StockList";
import CommentList from "./CommentList";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<StockList />} />
        <Route path="/stocks/:symbol/comments" element={<CommentList />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}