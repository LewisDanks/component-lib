import { z } from "zod";
import { createApp } from "vue";
import { PageNameSchema } from "./clientContext";
import { pages } from "./pages";

const PageMap = new Map(pages.map(p => [p.pageName, p] as const));

export function bootstrapApplication(raw: unknown) {
  const base = z.object({ pageName: PageNameSchema }).safeParse(raw);
  if (!base.success) return fail("Invalid or unknown pageName.", base.error);

  const mod = PageMap.get(base.data.pageName)!;

  const parsed = mod.schema.safeParse(raw);
  if (!parsed.success) return fail(`Invalid payload for "${mod.pageName}".`, parsed.error);

  createApp(mod.root, { context: parsed.data }).mount("#app");
}
function fail(message: string, error?: unknown) {
  console.error(message, error);
  const el = document.getElementById("app");
  if (el) el.innerHTML = `<pre>${message}</pre>`;
}
