
import { useEffect, useState } from "react";
import { api } from "../api";
import { Project } from "../types";

export default function Dashboard() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [title, setTitle] = useState("");

  useEffect(()=> {
    api.get("/projects").then(r=>setProjects(r.data));
  }, []);

  const create = async () => {
    if (!title.trim()) return;
    const res = await api.post("/projects", { title, description: ""});
    setProjects([...projects, res.data]);
    setTitle("");
  };

  const del = async (id:number) => {
    await api.delete(`/projects/${id}`);
    setProjects(projects.filter(p=>p.id!==id));
  };

  return (
    <div style={{padding:20}}>
      <h2>Dashboard</h2>
      <input placeholder="Project title" value={title} onChange={e=>setTitle(e.target.value)} />
      <button onClick={create}>Create</button>
      <ul>
        {projects.map(p=>(
          <li key={p.id}>
            <a href={"/projects/"+p.id}>{p.title}</a> &nbsp;
            <button onClick={()=>del(p.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
