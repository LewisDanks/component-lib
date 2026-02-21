import type { PageModule } from "../../app/pageModule";
import DashboardRoot from "./DashboardRoot.vue";
import { dashboardPageName, dashboardSchema } from "./schema";

export const DashboardPage = {
  pageName: dashboardPageName,
  schema: dashboardSchema,
  root: DashboardRoot,
} as const satisfies PageModule<typeof dashboardPageName, typeof dashboardSchema>;
