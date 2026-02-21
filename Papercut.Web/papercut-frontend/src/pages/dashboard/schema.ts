import { z } from "zod";

export const dashboardPageName = "Dashboard" as const;

export const dashboardSchema = z.object({
  pageName: z.literal(dashboardPageName),
  params: z.object({}),
  state: z.object({
    displayName: z.string(),
    signOutPath: z.string(),
  }),
});
