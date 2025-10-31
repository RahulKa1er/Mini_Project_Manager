
export interface Project {
  id: number;
  title: string;
  description?: string;
  createdAt: string;
  userId: number;
  tasks?: Task[];
}
export interface Task {
  id: number;
  title: string;
  dueDate?: string;
  isCompleted: boolean;
  projectId: number;
}
