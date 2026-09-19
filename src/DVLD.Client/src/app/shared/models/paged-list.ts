export class PagedList<T> {
  constructor(
    public readonly items: ReadonlyArray<T>,
    public readonly totalCount: number,
    public readonly pageNumber: number,
    public readonly pageSize: number,
  ) {}

  public get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  public get hasNextPage(): boolean {
    return this.pageNumber < this.totalPages;
  }

  public get hasPreviousPage(): boolean {
    return this.pageNumber > 1;
  }
}
