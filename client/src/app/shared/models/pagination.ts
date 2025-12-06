export type Pagination<T> = {
    items: T[];
    page: number;    
    pageSize: number;
    totalCount: number;
}