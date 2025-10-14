type RowObj = Record<string, unknown>;
interface PagedResponse<T> {
    items?: T[];
    total?: number;
}
interface UserGridView {
    recID?: number;
    userRefRecID: number;
    entityName: string;
    viewName: string;
    isDefault?: boolean;
    viewConfig: string;
    dataareaID: string;
}
declare const titleize: (s: string) => string;
declare const fmtCell: (v: unknown) => string;
declare function fetchJson<T>(url: string, options?: RequestInit): Promise<T>;
declare function inferColumns(sample: RowObj): string[];
//# sourceMappingURL=projects-dt.d.ts.map