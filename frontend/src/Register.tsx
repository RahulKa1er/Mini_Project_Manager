
import { useState } from "react";
import { api } from "../api";

export default function Register() {
  const [username, setU] = useState("");
  const [password, setP] = useState("");

  const handleRegister = async () => {
    try {
      const res = await api.post("/auth/register", { username, password });
      localStorage.setItem("token", res.data.token ?? res.data.token);
      window.location.href = "/dashboard";
    } catch (e) {
      alert("Register failed");
    }
  };

  return (
    <div style={{padding:20}}>
      <h2>Register</h2>
      <input placeholder="username" value={username} onChange={e=>setU(e.target.value)} />
      <input placeholder="password" type="password" value={password} onChange={e=>setP(e.target.value)} />
      <button onClick={handleRegister}>Register</button>
    </div>
  );
}
