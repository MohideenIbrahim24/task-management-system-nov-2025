export type Task = {
    id: number;
    title: string;
    description: string;
    dueDate: Date;
    status: 'pending' | 'in-progress' | 'completed';
    assignedToUserId: number;
    assignedUserName: string;
}