import { z } from "zod";
import { IndexPage } from "../pages/index/page";

export const pages = [IndexPage] as const;

type PageUnion = typeof pages[number];

export const PageNameSchema = z.enum(
  pages.map(p => p.pageName) as [PageUnion["pageName"], ...PageUnion["pageName"][]]
);

export const ClientContextSchema = z.discriminatedUnion(
  "pageName",
  pages.map(p => p.schema) as [
    PageUnion["schema"],
    ...PageUnion["schema"][]
  ]
);

export type ClientContext = z.infer<typeof ClientContextSchema>;
