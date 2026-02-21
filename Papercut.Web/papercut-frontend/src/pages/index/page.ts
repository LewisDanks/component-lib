import { z } from "zod";
import IndexRoot from "./IndexRoot.vue";
import type { PageModule } from "../../app/pageModule";

const schema = z.object({
  pageName: z.literal("Index"),
  params: z.object({
    returnUrl: z.string().optional(),
  }),
  state: z.object({
    count: z.number(),
  }),
});

export const IndexPage = {
  pageName: "Index",
  schema,
  root: IndexRoot,
} as const satisfies PageModule<"Index", typeof schema>;
