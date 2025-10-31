
import { useEffect, useState } from "react";
import { api } from "../api";
import { Task } from "../types";

export default function ProjectDetails() {
  const id = window.location.pathname.split("/").pop();
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState("");

  useEffect(()=>{
    api.get(`/projects/${id}/tasks`).then(r=>setTasks(r.data));
  }, [id]);

  const add = async () => {
    if(!title.trim()) return;
    const res = await api.post(`/projects/${id}/tasks`, { title, dueDate: null });
    setTasks([...tasks, res.data]);
    setTitle("");
  };

  const toggle = async (taskId:number) => {
    const res = await api.put(`/projects/${id}/tasks/${taskId}/toggle`);
    setTasks(tasks.map(t=> t.id===taskId ? res.data : t));
  };

  const del = async (taskId:number) => {
    await api.delete(`/projects/${id}/tasks/${taskId}`);
    setTasks(tasks.filter(t=>t.id!==taskId));
  };

  return (
    <div style={{padding:20}}>
      <h2>Project</h2>
      <input placeholder="New task" value={title} onChange={e=>setTitle(e.target.value)} />
      <button onClick={add}>Add task</button>
      <ul>
        {tasks.map(t=>(
          <li key={t.id}>
            <span onClick={()=>toggle(t.id)} style={{textDecoration: t.isCompleted ? "line-through" : "none", cursor:"pointer"}}>{t.title}</span>
            &nbsp;
            <button onClick={()=>del(t.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
