import { z } from "zod";

export const loginPageName = "Login" as const;

export const loginSchema = z.object({
  pageName: z.literal(loginPageName),
  params: z.object({
    returnUrl: z.string().nullable(),
  }),
  state: z.object({
    defaultDisplayName: z.string(),
    signInPath: z.string(),
    dashboardPath: z.string(),
  }),
});
