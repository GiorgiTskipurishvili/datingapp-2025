export type Pagination = {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPage: number;
}

export type PaginatedResult<T> ={
    items: T[];
    metadata:Pagination;
}