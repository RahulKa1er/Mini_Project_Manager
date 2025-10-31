
import { useState } from "react";
import { api } from "../api";

export default function Login() {
  const [username, setU] = useState("");
  const [password, setP] = useState("");

  const handleLogin = async () => {
    try {
      const res = await api.post("/auth/login", { username, password });
      localStorage.setItem("token", res.data.token ?? res.data.token);
      window.location.href = "/dashboard";
    } catch (e) {
      alert("Login failed");
    }
  };

  return (
    <div style={{padding:20}}>
      <h2>Login</h2>
      <input placeholder="username" value={username} onChange={e=>setU(e.target.value)} />
      <input placeholder="password" type="password" value={password} onChange={e=>setP(e.target.value)} />
      <button onClick={handleLogin}>Login</button>
    </div>
  );
}
