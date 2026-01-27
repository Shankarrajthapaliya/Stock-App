import { Routes, Route } from "react-router-dom";
import StockList from "./StockList";
import CommentList from "./CommentList";
import Login from "./Login";
import ProtectedRoute from "./auth/ProtectedRoute";

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />

      <Route element={<ProtectedRoute />}>
        <Route path="/" element={<StockList />} />
        <Route path="/stocks/:symbol/comments" element={<CommentList />} />
      </Route>
    </Routes>
  );
}

